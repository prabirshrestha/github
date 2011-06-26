using System;

namespace Github
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class GithubApiBadCredentialsException : GithubApiException
    {
        public GithubApiBadCredentialsException(string message)
            : base(message)
        {
        }
    }
}