using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnrollmentStation.Code
{
    public class YubikeyDetector : IDisposable
    {
        public static YubikeyDetector Instance { get; private set; }

        static YubikeyDetector()
        {
            Instance = new YubikeyDetector();
        }

        private bool _disposed;
        private bool _hasStarted;
        private ReaderWriterLockSlim _exclusiveLock = new ReaderWriterLockSlim();
        private Task _backgroundWorker;

        public bool CurrentState { get; private set; }

        private YubikeyDetector()
        {
            _backgroundWorker = new Task(BackgroundWork);
        }

        public event Action StateChanged;

        private void BackgroundWork()
        {
            while (!_disposed)
            {
                // Open full lock
                if (!_exclusiveLock.TryEnterWriteLock(1000))
                {
                    // Somebody else had it
                    continue;
                }

                bool hadChange;
                try
                {
                    // Read the state
                    bool hadDevice = YubikeyNeoManager.Instance.RefreshDevice();
                    hadChange = hadDevice != CurrentState;

                    CurrentState = hadDevice;
                }
                finally
                {
                    _exclusiveLock.ExitWriteLock();
                }

                if (hadChange)
                    // Change
                    OnStateChanged();

                // Wait up to 1s
                Thread.Sleep(1000);
            }
        }

        public void Start()
        {
            if (_hasStarted)
                return;

            _hasStarted = true;
            _backgroundWorker.Start();
        }

        public ExclusiveLock GetExclusiveLock()
        {
            return new ExclusiveLock(this);
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke();
        }

        public void Dispose()
        {
            _disposed = true;

            _backgroundWorker.Wait();
        }

        public class ExclusiveLock : IDisposable
        {
            private readonly YubikeyDetector _yubikeyDetector;

            internal ExclusiveLock(YubikeyDetector yubikeyDetector)
            {
                _yubikeyDetector = yubikeyDetector;
                _yubikeyDetector._exclusiveLock.EnterWriteLock();
            }

            public void Dispose()
            {
                _yubikeyDetector._exclusiveLock.ExitWriteLock();
            }
        }
    }
}