using System;
using System.Threading;
using System.Threading.Tasks;

namespace NappiSite.EasyAsync
{
    public class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim _lock;
        private bool _disposedValue;

        public AsyncLock()
        {
            _lock = new SemaphoreSlim(1, 1);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal Task WaitAsync()
        {
            return _lock.WaitAsync();
        }

        public int Release()
        {
            return _lock.Release();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing) _lock.Dispose();

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }
    }
}