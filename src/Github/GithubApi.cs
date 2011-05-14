using System;
using System.Collections.Generic;
using System.Net;
using FluentHttp;

namespace Github
{
    public class GithubApi
    {
        public GithubApi()
        {
            Version = "v2";
        }

        public string Version { get; set; }

        public IWebProxy Proxy { get; set; }

        public object Get(string path, IDictionary<string, object> parameters)
        {
            return GithubApiRequest("GET", path, parameters, null);
        }

        public object Get(string path)
        {
            return Get(path, null);
        }

        protected virtual object GithubApiRequest(string method, string path, IDictionary<string, object> parameters, Type resultType)
        {
            // save the response to this stream.
            var responseStream = new System.IO.MemoryStream();

            var request =
                PrepareRequest(method, path, parameters)
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseStream);

            // make async behave like sync methods by calling EndRequest right after BeginRequest.
            var response = request.EndExecute(request.BeginExecute(null, null));

            // process the response
            Exception exception;
            var result = ProcessResponse(response, responseStream, resultType, out exception);
            if (exception != null)
            {
                throw exception;
            }

            return result;
        }

        protected virtual FluentHttpRequest PrepareRequest(string method, string path, IDictionary<string, object> parameters)
        {
            var request = new FluentHttpRequest()
                .BaseUrl(string.Format("http://github.com/api/{0}/json", Version))
                .ResourcePath(path)
                .Method(method)
                .Proxy(Proxy);

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

        protected virtual object ProcessResponse(FluentHttpResponse response, System.IO.Stream responseStream, Type resultType, out Exception exception)
        {
            // don't throw exception in this method but send it as an out parameter.
            // we can reuse this method for async methods too.
            // so the caller of this method decides whether to throw or not.
            exception = null;

            // FluentHttpRequest has ended.
            switch (response.ResponseStatus)
            {
                case ResponseStatus.Completed:
                    // convert the response stream to string.
                    responseStream.Seek(0, System.IO.SeekOrigin.Begin);
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
                        exception = ExceptionFactory.GetException(responseString);
                        return null;
                    }

                default:
                    // we should never reach here.
                    // incase we reach here then there is bug in the code.
                    // coz we can never have ResponseStatus as Cancelled or Non at this point.
                    throw new Exception("Github exception occurred.");
            }
        }
    }
}