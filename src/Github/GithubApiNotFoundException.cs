using System;

namespace Github
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class GithubApiNotFoundException : GithubApiException
    {
        public GithubApiNotFoundException(string message)
            : base(message)
        {
        }
    }
}