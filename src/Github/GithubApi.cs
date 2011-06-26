
namespace Github
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Github.FluentHttp.Authenticators;
    using global::FluentHttp;

    public class GithubApi
    {
        private readonly GithubApiVersion _version;
        private readonly IGithubAuthenticator _authentication;

        #region ctors

        public GithubApi()
            : this(GithubApiVersion.Unknown)
        {
        }

        public GithubApi(GithubApiVersion version)
            : this(version, null)
        {
        }

        public GithubApi(IGithubAuthenticator authentication)
            : this(GithubApiVersion.Unknown, authentication)
        {
        }

        public GithubApi(GithubApiVersion version, IGithubAuthenticator authentication)
        {
            _version = version;
            _authentication = authentication;
        }

        #endregion

        public IGithubAuthenticator Authentication
        {
            get { return _authentication; }
        }

        public GithubApiVersion Version
        {
            get { return _version; }
        }

        public IWebProxy Proxy { get; set; }

        #region Api Calls

#if !SILVERLIGHT

        #region Get

        public object Get(GithubApiVersion version, string path, IDictionary<string, object> parameters)
        {
            return Api(version, "GET", path, parameters, null);
        }

        public object Get(string path, IDictionary<string, object> parameters)
        {
            return Get(Version, path, parameters);
        }

        public object Get(GithubApiVersion version, string path)
        {
            return Get(version, path, null);
        }

        public object Get(string path)
        {
            return Get(Version, path);
        }

        #endregion

        #region Post

        public object Post(GithubApiVersion version, string path, IDictionary<string, object> parameters)
        {
            return Api(version, "POST", path, parameters, null);
        }

        public object Post(string path, IDictionary<string, object> parameters)
        {
            return Post(Version, path, parameters);
        }

        public object Post(GithubApiVersion version, string path, IList<object> parameters)
        {
            return Api(version, "POST", path, parameters, null);
        }

        public object Post(string path, IList<object> parameters)
        {
            return Post(Version, path, parameters);
        }

        #endregion

        #region Put

        public object Put(GithubApiVersion version, string path)
        {
            return Api(version, "PUT", path, null, null);
        }

        public object Put(string path)
        {
            return Put(Version, path);
        }

        #endregion

        #region Patch

        public object Patch(GithubApiVersion version, string path, IDictionary<string, object> parameters)
        {
            return Api(version, "PATCH", path, parameters, null);
        }

        public object Patch(string path, IDictionary<string, object> parameters)
        {
            return Patch(Version, path, parameters);
        }

        #endregion

        #region Delete

        public object Delete(GithubApiVersion version, string path, IList<object> parameters)
        {
            return Api(version, "DELETE", path, parameters, null);
        }

        public object Delete(string path, IList<object> parameters)
        {
            return Delete(Version, path, parameters);
        }

        public object Delete(GithubApiVersion verion, string path)
        {
            return Delete(verion, path, null);
        }

        public object Delete(string path)
        {
            return Delete(Version, path);
        }

        #endregion

        #region Head

        public object Head(GithubApiVersion version, string path, IDictionary<string, object> parameters)
        {
            return Api(version, "HEAD", path, parameters, null);
        }

        public object Head(string path, IDictionary<string, object> parameters)
        {
            return Head(Version, path, parameters);
        }

        public object Head(string path)
        {
            return Head(path, null);
        }

        #endregion

#endif
        #region Get

        public void GetAsync(GithubApiVersion version, string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "GET", path, parameters, null, state, callback);
        }

        public void GetAsync(string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            GetAsync(Version, path, parameters, state, callback);
        }

        public void GetAync(GithubApiVersion version, string path, object state, GithubAsyncCallback callback)
        {
            GetAsync(version, path, null, state, callback);
        }

        public void GetAsync(string path, object state, GithubAsyncCallback callback)
        {
            GetAsync(Version, path, null, state, callback);
        }

        #endregion

        #region Post

        public void PostAsync(GithubApiVersion version, string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "POST", path, parameters, null, state, callback);
        }

        public void PostAync(string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            PostAsync(Version, path, parameters, state, callback);
        }

        public void PostAsync(GithubApiVersion version, string path, IList<object> parameters, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "POST", path, parameters, null, state, callback);
        }

        public void Post(string path, IList<object> parameters, object state, GithubAsyncCallback callback)
        {
            PostAsync(Version, path, parameters, state, callback);
        }

        #endregion

        #region Put

        public void PutAsync(GithubApiVersion version, string path, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "PUT", path, null, null, state, callback);
        }

        public void PutAsync(string path, object state, GithubAsyncCallback callback)
        {
            PutAsync(Version, path, state, callback);
        }

        #endregion

        #region Patch

        public void PatchAsync(GithubApiVersion version, string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "PATCH", path, parameters, null, state, callback);
        }

        public void PatchAsync(string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            PatchAsync(Version, path, parameters, state, callback);
        }

        #endregion

        #region Delete

        public void DeleteAsync(GithubApiVersion version, string path, IList<object> parameters, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "DELETE", path, parameters, null, state, callback);
        }

        public void DeleteAsync(string path, IList<object> parameters, object state, GithubAsyncCallback callback)
        {
            DeleteAsync(Version, path, parameters, state, callback);
        }

        public void DeleteAsync(GithubApiVersion verion, string path, object state, GithubAsyncCallback callback)
        {
            DeleteAsync(verion, path, null, state, callback);
        }

        public void DeleteAsync(string path, object state, GithubAsyncCallback callback)
        {
            DeleteAsync(Version, path, state, callback);
        }

        #endregion

        #region Head

        public void HeadAsync(GithubApiVersion version, string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            ApiAsync(version, "HEAD", path, parameters, null, state, callback);
        }

        public void HeadAsync(string path, IDictionary<string, object> parameters, object state, GithubAsyncCallback callback)
        {
            HeadAsync(Version, path, parameters ,state, callback);
        }

        public void HeadAsync(string path, object state, GithubAsyncCallback callback)
        {
            HeadAsync(path, null, state, callback);
        }

        #endregion

        #endregion

        #region HttpWebRequest Helper methods

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual object Api(GithubApiVersion version, string method, string path, object parameters, Type resultType)
        {
            if (!(version == GithubApiVersion.V2 || version == GithubApiVersion.V3))
                throw new ArgumentOutOfRangeException("version", "Unknown version");

            Stream responseStream = null;
            if (!method.Equals("HEAD", StringComparison.OrdinalIgnoreCase))
                responseStream = new MemoryStream();

            var request = PrepareRequest(version, method, path, parameters, responseStream);

            // execute the request.
            var ar = request.Execute();

            Exception exception;

            // process the response
            var result = ProcessResponse(version, ar, responseStream, resultType, out exception);

            if (exception == null)
                return result;

            throw exception;
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual void ApiAsync(GithubApiVersion version, string method, string path, object parameters, Type resultType, object state, GithubAsyncCallback callback)
        {
            if (!(version == GithubApiVersion.V2 || version == GithubApiVersion.V3))
                throw new ArgumentOutOfRangeException("version", "Unknown version");

            Stream responseStream = null;
            if (!method.Equals("HEAD", StringComparison.OrdinalIgnoreCase))
                responseStream = new MemoryStream();

            var request = PrepareRequest(version, method, path, parameters, responseStream);

            request.ExecuteAsync(
                ar =>
                {
                    Exception exception = ar.Exception;
                    object result = null;

                    if (exception == null)
                        result = ProcessResponse(version, ar, responseStream, resultType, out exception);

                    if (callback != null)
                    {
                        callback(new GithubAsyncResult(ar.IsCompleted, ar.AsyncWaitHandle,
                                                       ar.CompletedSynchronously, ar.IsCancelled, result, ar.AsyncState,
                                                       exception));
                    }

                }, state);
        }

        internal virtual FluentHttpRequest PrepareRequest(GithubApiVersion version, string method, string path, object parameters, Stream responseStream)
        {
            var request = new FluentHttpRequest()
                .ResourcePath(path)
                .Method(method)
                .Proxy(Proxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseStream));

            request.BaseUrl(version == GithubApiVersion.V3
                                ? "https://api.github.com"
                                : "https://github.com/api/v2/json");

            string bearerToken = null;

            IDictionary<string, object> dictionaryParameters = null;
            if (parameters is IDictionary<string, object>)
                dictionaryParameters = (IDictionary<string, object>)parameters;

            // give priority to bearer_token then access_token specified in parameters.
            if (dictionaryParameters != null)
            {
                if (dictionaryParameters.ContainsKey("bearer_token"))
                {
                    bearerToken = dictionaryParameters["bearer_token"].ToString();
                    dictionaryParameters.Remove(bearerToken);
                }
                else if (dictionaryParameters.ContainsKey("access_token"))
                {
                    bearerToken = dictionaryParameters["access_token"].ToString();
                    dictionaryParameters.Remove(bearerToken);
                }
            }

            if (Authentication != null)
            {
                if (string.IsNullOrEmpty(bearerToken) && Authentication is GithubOAuthAuthenticator)
                {
                    var oauth2 = (GithubOAuthAuthenticator)Authentication;
                    bearerToken = oauth2.AccessToken;
                }

                if (string.IsNullOrEmpty(bearerToken))
                {
                    if (Authentication is GithubBasicAuthenticator)
                    {
                        var basicAuth = (GithubBasicAuthenticator)Authentication;
                        request.AuthenticateUsing(new HttpBasicAuthenticator(basicAuth.Username, basicAuth.Password));
                    }
                    else
                    {
                        throw new NotSupportedException("Authentication not supported.");
                    }
                }
                else
                {
                    request.AuthenticateUsing(new OAuth2AuthorizationRequestHeaderBearerAuthenticator(bearerToken));
                }
            }

            if (method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // for GET, all parameters goes as querystring
                request.QueryStrings(qs => qs.Add(dictionaryParameters));
            }
            else
            {
                return request
                    .Headers(h => h.Add("Content-Type", "application/json"))
                    .Body(b =>
                              {
                                  if (parameters != null)
                                      b.Append(JsonSerializer.Current.SerializeObject(parameters));
                              });
            }

            return request;
        }

        internal virtual object ProcessResponse(GithubApiVersion version, FluentHttpAsyncResult asyncResult, Stream responseStream, Type resultType, out Exception exception)
        {
            // FluentHttpRequest has ended.

            // don't throw exception in this method but send it as an out parameter.
            // we can reuse this method for async methods too.
            // so the caller of this method decides whether to throw or not.
            exception = asyncResult.Exception;

            if (exception != null)
            {
                if (responseStream != null)
                    responseStream.Dispose();
                return null;
            }

            if (asyncResult.IsCancelled)
            {
                exception = new NotImplementedException();
                return null;
            }

            var response = asyncResult.Response;

            // async request completed
            if (response.HttpWebResponse.StatusCode == HttpStatusCode.BadGateway)
            {
                if (responseStream != null)
                    responseStream.Dispose();
                exception = asyncResult.InnerException;
                return null;
            }

            var httpWebRequestHeaders = response.HttpWebResponse.Headers;
            var headers = new SimpleJson.JsonObject();
            foreach (var headerKey in httpWebRequestHeaders.AllKeys)
                headers.Add(headerKey, httpWebRequestHeaders[headerKey]);

            var result = new SimpleJson.JsonObject
                             {
                                 {"code", (int) response.HttpWebResponse.StatusCode},
                                 {"headers", headers}
                             };

            if (response.Request.GetMethod().Equals("HEAD", StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            // convert the response stream to string.
            responseStream.Seek(0, SeekOrigin.Begin);

            switch (response.HttpWebResponse.StatusCode)
            {
                case HttpStatusCode.Created:
                case HttpStatusCode.OK:
                    try
                    {
                        if (headers.ContainsKey("X-GitHub-Blob-Sha"))
                        {
                            // responseStream is a binary data

                            // responseStream is always memory stream
                            var ms = (MemoryStream)responseStream;
                            var data = ms.ToArray();
                            ms.Dispose();

                            result["body"] = data;
                        }
                        else
                        {
                            // responseStream is a string.

                            var responseString = FluentHttpRequest.ToString(responseStream);
                            // we got the response string already, so dispose the memory stream.
                            responseStream.Dispose();

                            result["body"] = (resultType == null)
                                                 ? JsonSerializer.Current.DeserializeObject(responseString)
                                                 : JsonSerializer.Current.DeserializeObject(responseString, resultType);
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                        return null;
                    }
                case HttpStatusCode.NoContent:
                    responseStream.Dispose();
                    return result;
                default:
                    {
                        var responseString = FluentHttpRequest.ToString(responseStream);
                        responseStream.Dispose();

                        // Github api responded with an error.
                        object json;
                        exception = ExceptionFactory.GetException(responseString, version, response, out json);
                        result["body"] = json;
                        return result;
                    }
            }
        }

        #endregion

        #region Utils method

        /// <summary>
        /// Parse a URL query and fragment parameters into a key-value bundle.
        /// </summary>
        /// <param name="query">
        /// The URL query to parse.
        /// </param>
        /// <returns>
        /// Returns a dictionary of keys and values for the querystring.
        /// </returns>
        internal static IDictionary<string, object> ParseUrlQueryString(string query)
        {
            var result = new Dictionary<string, object>();

            // if string is null, empty or whitespace
            if (string.IsNullOrEmpty(query) || query.Trim().Length == 0)
            {
                return result;
            }

            string decoded = FluentHttpRequest.HtmlDecode(query);
            int decodedLength = decoded.Length;
            int namePos = 0;
            bool first = true;

            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                    {
                        namePos++;
                    }
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = FluentHttpRequest.UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1));
                }

                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }

                value = FluentHttpRequest.UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos));

                if (!string.IsNullOrEmpty(name))
                {
                    result[name] = value;
                }

                if (namePos == -1)
                {
                    break;
                }
            }

            return result;
        }

        #endregion

    }
}