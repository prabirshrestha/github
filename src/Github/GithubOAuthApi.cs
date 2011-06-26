
namespace Github
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using global::FluentHttp;

    public class GithubOAuthApi
    {
        public virtual string ClientId { get; set; }
        public virtual string ClientSecret { get; set; }
        public virtual Uri RedirectUri { get; set; }

        public virtual IWebProxy Proxy { get; set; }

        public virtual GithubOAuthResult ParseResult(Uri uri)
        {
            return ParseResult(uri, true);
        }

        internal static GithubOAuthResult ParseResult(Uri uri, bool throws)
        {
            IDictionary<string, object> parameters = null;
            try
            {
                bool found = false;
                if (!string.IsNullOrEmpty(uri.Fragment))
                {
                    // #access_token and expires_in are in fragment
                    var fragment = uri.Fragment.Substring(1);
                    parameters = GithubApi.ParseUrlQueryString(fragment);
                    if (parameters.ContainsKey("access_token"))
                        found = true;
                }

                // code, state, error_reason, error and error_description are in query
                // ?error=user_denied
                var queryPart = GithubApi.ParseUrlQueryString(uri.Query);
                if (queryPart.ContainsKey("code") || (queryPart.ContainsKey("error")))
                    found = true;

                if (found)
                {
                    parameters = FluentHttpRequest.Merge(parameters, queryPart);
                    return new GithubOAuthResult(parameters);
                }
            }
            catch (Exception)
            {
                if (throws)
                    throw;
                return null;
            }

            if (throws)
            {
                throw new InvalidOperationException("Could not parse authentication url.");
            }

            return null;
        }

        public virtual Uri GetLoginUrl(IDictionary<string, object> parameters)
        {
            var defaultParameters = new Dictionary<string, object>();
            defaultParameters["client_id"] = ClientId;
            defaultParameters["redirect_uri"] = RedirectUri;

            var mergedParameters = FluentHttpRequest.Merge(defaultParameters, parameters);

            // check if client_id and redirect_uri is not null or empty.
            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
                throw new ArgumentNullException("parameters", "client_id requried.");
            if (mergedParameters["redirect_uri"] == null || string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
                throw new ArgumentNullException("parameters", "redirect_uri requried.");

            // seems like if we don't do this and rather pass the original uri object,
            // it seems to have http://localhost:80/ instead of
            // http://localhost/
            // notice the port number, that shouldn't be there.
            // this seems to happen for iis hosted apps.
            mergedParameters["redirect_uri"] = mergedParameters["redirect_uri"].ToString();

            var request = new FluentHttpRequest()
                .BaseUrl("https://github.com/login/oauth/authorize")
                .QueryStrings(qs => qs.Add(mergedParameters));

            return new Uri(request.BuildRequestUrl());
        }

        public virtual object ExchangeCodeForAccessToken(string code, IDictionary<string, object> parameters)
        {
            var responseStream = new MemoryStream();
            var request = PrepareRequest(
                "POST",
                "https://github.com/login/oauth/access_token",
                BuildExchangeCodeForAccessTokenParmaters(code, parameters),
                responseStream);

            // execute the request.
            var ar = request.Execute();

            Exception exception;

            // process the response
            var result = ProcessExchangeCodeForAccessTokenResponse(ar, responseStream, out exception);

            if (exception == null)
                return result;

            throw exception;
        }

        private object ProcessExchangeCodeForAccessTokenResponse(FluentHttpAsyncResult asyncResult, MemoryStream responseStream, out Exception exception)
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
                // async request completed
                // convert the response stream to string.
                responseStream.Seek(0, SeekOrigin.Begin);
                var responseString = FluentHttpRequest.ToString(responseStream);

                // we got the response string already, so dispose the memory stream.
                responseStream.Dispose();

                if (response.HttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    var returnValue = GithubApi.ParseUrlQueryString(responseString);

                    if (returnValue.ContainsKey("error"))
                    {
                        // Github api responded with an error.
                        exception = new GithubApiException(returnValue["error"].ToString());
                        return null;
                    }
                    return returnValue;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private IDictionary<string, object> BuildExchangeCodeForAccessTokenParmaters(string code, IDictionary<string, object> parameters)
        {
            var defaultParameters = new Dictionary<string, object>();
            defaultParameters["client_id"] = ClientId;
            defaultParameters["redirect_uri"] = RedirectUri;
            defaultParameters["client_secret"] = ClientSecret;
            defaultParameters["code"] = code;

            var mergedParameters = FluentHttpRequest.Merge(defaultParameters, parameters);

            // check if client_id, redirect_uri,client_secret and code is not null or empty.
            if (mergedParameters["client_id"] == null || string.IsNullOrEmpty(mergedParameters["client_id"].ToString()))
                throw new ArgumentNullException("parameters", "client_id requried.");
            if (mergedParameters["redirect_uri"] == null || string.IsNullOrEmpty(mergedParameters["redirect_uri"].ToString()))
                throw new ArgumentNullException("parameters", "redirect_uri requried.");
            if (mergedParameters["client_secret"] == null || string.IsNullOrEmpty(mergedParameters["client_secret"].ToString()))
                throw new ArgumentNullException("parameters", "client_secret requried.");
            if (mergedParameters["code"] == null || string.IsNullOrEmpty(mergedParameters["code"].ToString()))
                throw new ArgumentNullException("parameters", "code requried.");

            return mergedParameters;
        }

        internal FluentHttpRequest PrepareRequest(string method, string url, IDictionary<string, object> parameters, Stream responseStream)
        {
            /*
            if (!method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentOutOfRangeException("method", "Only POST http method supported.");
            }
            */

            var request = new FluentHttpRequest()
                .BaseUrl(url)
                .Method(method)
                .Headers(h => h.Add("Content-Type", "application/x-www-form-urlencoded"))
                .Body(b => b.Append(parameters))
                .Proxy(Proxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseStream)); ;

            return request;
        }
    }
}