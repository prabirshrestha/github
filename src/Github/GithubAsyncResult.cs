using System;
using System.Threading;

namespace Github
{
    public delegate void GithubAsyncCallback(GithubAsyncResult asyncResult);

    public class GithubAsyncResult : IAsyncResult
    {
        private readonly bool _isCompleted;
        private readonly WaitHandle _asyncWaitHandle;
        private readonly object _asyncState;
        private readonly bool _ccompletedSynchronously;
        private readonly bool _isCancelled;
        private readonly object _result;
        private readonly Exception _exception;

        public GithubAsyncResult(bool isCompleted, WaitHandle asyncWaitHandle, bool completedSynchronously, bool isCancelled, object result, object asyncState, Exception exception)
        {
            _isCompleted = isCompleted;
            _asyncWaitHandle = asyncWaitHandle;
            _asyncState = asyncState;
            _ccompletedSynchronously = completedSynchronously;
            _isCancelled = isCancelled;
            _result = result;
            _exception = exception;
        }

        public object Result
        {
            get { return _result; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public bool IsCancelled
        {
            get { return _isCancelled; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return _asyncWaitHandle; }
        }

        public object AsyncState
        {
            get { return _asyncState; }
        }

        public bool CompletedSynchronously
        {
            get { return _ccompletedSynchronously; }
        }
    }
}