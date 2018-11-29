using System;
namespace ChristmasPickNotifier.Notifier
{
    public abstract class NotifierResult
    {
        private bool _isSuccess;
        private string _message;

        protected NotifierResult(bool isSuccess, string message)
        {
            _isSuccess = isSuccess;
            _message = message;
        }

        public bool IsSuccess()
        {
            return _isSuccess;
        }

        public string Message
        { 
            get
            {
                return _message;
            }
            protected set
            {
                _message = value;
            }
        }
    }

    public class NotifierResultFactory : NotifierResult
    {
        public static NotifierResult Success = new NotifierResultFactory(true, string.Empty);
        private NotifierResultFactory(bool isSuccess, string message) : base(isSuccess, message)
        {
        }

        public static NotifierResult CreateFailed(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            return new NotifierResultFactory(false, message);
        }
    }
}
