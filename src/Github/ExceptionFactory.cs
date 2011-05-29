
namespace Github
{
    using System;
    using System.Collections.Generic;
    using FluentHttp;

    internal class ExceptionFactory
    {
        public static Exception GetException(string responseString, string version, FluentHttpResponse response)
        {
            try
            {
                var json = (IDictionary<string, object>)JsonSerializer.Current.DeserializeObject(responseString);

                return version == "v3" ? GetV3Exception(json, response) : GetV2Exception(json, response);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static Exception GetV3Exception(IDictionary<string, object> json, FluentHttpResponse response)
        {
            GithubApiException exception;
            if (json.ContainsKey("message"))
            {
                exception = new GithubApiException((string)json["message"]);
                return exception;
            }

            return new NotImplementedException();
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