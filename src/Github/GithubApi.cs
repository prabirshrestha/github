using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using FluentHttp;

namespace Github
{
    public class GithubApi
    {
        private readonly string _version;

        public GithubApi(string version)
        {
            if (string.IsNullOrEmpty(version))
                throw new ArgumentNullException("version");

            _version = version;
        }

        public string Version { get { return _version; } }

        public IWebProxy Proxy { get; set; }

        public static GithubApi v2()
        {
            return new GithubApi("v2");
        }

        public static GithubApi v3()
        {
            return new GithubApi("v3");
        }

        #region Api Calls

        public object Get(string path, IDictionary<string, object> parameters)
        {
            return GithubApiRequest("GET", path, parameters, null);
        }

        public object Get(string path)
        {
            return Get(path, null);
        }

        #endregion

        #region HttpWebRequest Helper methods

        protected virtual object GithubApiRequest(string method, string path, IDictionary<string, object> parameters,
                                                  Type resultType)
        {
            // save the response to this stream.
            var responseStream = new MemoryStream();

            var request =
                PrepareRequest(method, path, parameters)
                    .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseStream));

            // execute the request.
            var ar = request.Execute();

            Exception exception;

            // process the response
            var result = ProcessResponse(ar, responseStream, resultType, out exception);

            if (exception == null)
                return result;

            throw exception;
        }

        protected virtual FluentHttpRequest PrepareRequest(string method, string path,
                                                           IDictionary<string, object> parameters)
        {
            var request = new FluentHttpRequest()
                .ResourcePath(path)
                .Method(method)
                .Proxy(Proxy);

            if (Version == "v3")
                request.BaseUrl("https://api.github.com");
            else
                request.BaseUrl(string.Format("https://github.com/api/{0}/json", Version)); // v2

            // i think FluentHttp should have a new method called Parameters()
            // which automatically puts the values to either querystring or
            // body depending on the http method
            if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // for GET, all parameters goes as querystring
                request.QueryStrings(qs => qs.Add(parameters));
            }
            else
            {
                request
                    .Headers(h => h.Add("Content-Type", "application/x-www-form-urlencoded"))
                    .Body(b => b.Append(parameters));
            }

            return request;
        }

        protected virtual object ProcessResponse(FluentHttpAsyncResult asyncResult, MemoryStream responseStream,
                                                 Type resultType, out Exception exception)
        {
            // don't throw exception in this method but send it as an out parameter.
            // we can reuse this method for async methods too.
            // so the caller of this method decides whether to throw or not.
            exception = asyncResult.Exception;

            // FluentHttpRequest has ended.

            if (exception != null)
            {
                if (responseStream != null)
                    responseStream.Dispose();

                return null;
            }

            var response = asyncResult.Response;

            if (asyncResult.IsCancelled)
            {
                exception = new NotImplementedException();
                return null;
            }
            else
            {
                // asyncResult completed

                // convert the response stream to string.
                responseStream.Seek(0, SeekOrigin.Begin);
                var responseString = FluentHttpRequest.ToString(responseStream);

                // we got the response string already, so dispose the memory stream.
                responseStream.Dispose();

                if (response.HttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    // Github api executed successfully.
                    return (resultType == null)
                               ? JsonSerializer.Current.DeserializeObject(responseString)
                               : JsonSerializer.Current.DeserializeObject(responseString, resultType);
                }
                else
                {
                    // Github api responded with an error.
                    exception = ExceptionFactory.GetException(responseString, Version, response);
                    return null;
                }
            }
        }

        #endregion

    }
}