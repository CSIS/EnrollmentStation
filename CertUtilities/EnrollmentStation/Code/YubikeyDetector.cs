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

                try
                {
                    // Read the state
                    using (YubikeyNeoManager neo = new YubikeyNeoManager())
                    {
                        bool hadDevice = neo.RefreshDevice();
                        bool hadChange = hadDevice != CurrentState;

                        CurrentState = hadDevice;
                        if (hadChange)
                            // Change
                            OnStateChanged();
                    }
                }
                finally
                {
                    _exclusiveLock.ExitWriteLock();
                }

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
            Task.Factory.StartNew(() =>
            {
                Action handler = StateChanged;
                if (handler != null)
                    handler();
            });
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