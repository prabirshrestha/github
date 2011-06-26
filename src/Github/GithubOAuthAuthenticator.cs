namespace Github
{
    public class GithubOAuthAuthenticator : IGithubAuthenticator
    {
        private readonly string _accessToken;

        public GithubOAuthAuthenticator(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new System.ArgumentNullException("accessToken");

            _accessToken = accessToken;
        }

        public string AccessToken
        {
            get { return _accessToken; }
        }
    }
}