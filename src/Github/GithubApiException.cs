using System;

namespace Github
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class GithubApiException : Exception
    {
        public GithubApiException(string message)
            : base(message)
        {
        }
    }
}