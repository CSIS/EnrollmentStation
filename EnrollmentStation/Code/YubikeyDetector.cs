using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities.Collections;

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
        private Task _backgroundWorker;

        private HashSet<string> _previousList;

        public bool CurrentState { get; private set; }

        private YubikeyDetector()
        {
            _previousList = new HashSet<string>();
            _backgroundWorker = new Task(BackgroundWork);
        }

        public event Action StateChanged;

        private void BackgroundWork()
        {
            List<string> missing = new List<string>();
            List<string> @new = new List<string>();

            while (!_disposed)
            {
                HashSet<string> devices = new HashSet<string>(YubicoLib.YubikeyNeo.YubikeyNeoManager.Instance.ListDevices());

                missing.AddRange(_previousList.Except(devices));
                @new.AddRange(devices.Except(_previousList));

                bool hadChange = false;
                foreach (string device in missing)
                {
                    hadChange = true;

                    _previousList.Remove(device);
                }

                foreach (string device in @new)
                {
                    hadChange = true;

                    _previousList.Add(device);
                }

                missing.Clear();
                @new.Clear();
                
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
        
        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke();
        }

        public void Dispose()
        {
            _disposed = true;

            _backgroundWorker.Wait();
        }
    }
}