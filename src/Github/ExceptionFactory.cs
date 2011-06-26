
namespace Github
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using global::FluentHttp;

    internal class ExceptionFactory
    {
        public static Exception GetException(string responseString, GithubApiVersion version, FluentHttpResponse response, out object json)
        {
            try
            {
                json = JsonSerializer.Current.DeserializeObject(responseString);

                return version == GithubApiVersion.V3 ? GetV3Exception((IDictionary<string, object>)json, response) : GetV2Exception((IDictionary<string, object>)json, response);
            }
            catch (Exception ex)
            {
                json = null;
                return ex;
            }
        }

        private static Exception GetV3Exception(IDictionary<string, object> json, FluentHttpResponse response)
        {
            if (json.ContainsKey("message"))
            {
                var message = (string)json["message"];

                switch (response.HttpWebResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new GithubApiNotFoundException(message);
                    case HttpStatusCode.Unauthorized:
                        return new GithubApiBadCredentialsException(message);
                    default:
                        return new GithubApiException(message);
                }
            }

            throw new ApplicationException("Invalid error message");
        }

        private static Exception GetV2Exception(IDictionary<string, object> json, FluentHttpResponse response)
        {
            GithubApiException exception;
            if (json.ContainsKey("error"))
            {
                exception = new GithubApiException((string)json["error"]);
                return exception;
            }

            return new NotImplementedException();
        }
    }
}