
namespace Github
{
    using System.Collections.Generic;

    public class GithubOAuthResult
    {
        private readonly string _error;
        private readonly string _code;

        internal protected GithubOAuthResult(IDictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("error"))
            {
                _error = parameters["error"].ToString();
                return;
            }

            if (parameters.ContainsKey("code"))
            {
                _code = parameters["code"].ToString();
            }
        }

        public virtual string Code
        {
            get { return _code; }
        }

        public virtual string Error
        {
            get { return _error; }
        }

        public virtual bool IsSuccess
        {
            get { return string.IsNullOrEmpty(Error) && !string.IsNullOrEmpty(Code); }
        }
    }
}