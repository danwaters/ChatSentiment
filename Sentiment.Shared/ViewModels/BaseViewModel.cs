using System;
using System.Threading;

namespace Sentiment
{
    public class BaseViewModel : BaseNotify
    {
        public BaseViewModel()
        {
            //Log.Instance.WriteLine($"{GetType().Name} created");
        }

        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler IsBusyChanged;
        public event EventHandler RefreshedChat;

        bool _isBusy;
        public virtual bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetPropertyChanged(ref _isBusy, value))
                {
                    SetPropertyChanged(nameof(IsNotBusy));
                    OnIsBusyChanged();
                    IsBusyChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public bool IsNotBusy => !IsBusy;

        bool isRefreshingChat;
        public bool IsRefreshingChat
        {
            get { return isRefreshingChat; }
            set { SetPropertyChanged(ref isRefreshingChat, value); }
        }

        string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { SetPropertyChanged(ref _currentState, value); }
        }

        public bool WasCancelledAndReset
        {
            get
            {
                var cancelled = _cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested;

                if (cancelled)
                    ResetCancellationToken();

                return cancelled;
            }
        }

        protected virtual void OnIsBusyChanged() { }

        public CancellationToken CancellationToken
        {
            get
            {
                if (_cancellationTokenSource == null)
                    _cancellationTokenSource = new CancellationTokenSource();

                return _cancellationTokenSource.Token;
            }
        }

        public void ResetCancellationToken()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public virtual void CancelTasks()
        {
            if (!_cancellationTokenSource.IsCancellationRequested && CancellationToken.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        public virtual void OnNotificationReceived(NotificationEventArgs args)
        {
            //Log.Instance.WriteLine($"Notification received on {GetType().Name}");
        }

        public virtual void OnResume()
        {
            //Log.Instance.WriteLine($"App resumed on {GetType().Name}");
        }

        public virtual void OnSleep()
        {
            //Log.Instance.WriteLine($"App slept on {GetType().Name}");
        }

        public virtual void NotifyPropertiesChanged()
        {
        }
    }

    public class SendResponse<T>
    {
        public SendResponse() { }

        public SendResponse(T result)
        {
            Result = result;
            Status = ResponseStatus.Success;
        }

        public T Result { get; set; }
        public ResponseStatus Status { get; set; }
        public Exception Exception { get; set; }
    }

    public enum ResponseStatus
    {
        None,
        Success,
        Fail,
        NoConnection,
    }
}
