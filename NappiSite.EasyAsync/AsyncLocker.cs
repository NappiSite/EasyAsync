using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NappiSite.EasyAsync
{
    public class AsyncLocker : IDisposable
    {
        private readonly ConcurrentDictionary<string, AsyncLock> _locks;
        private bool _disposedValue;
        public AsyncLocker()
        {
            _locks = new ConcurrentDictionary<string, AsyncLock>();
        }

        public async Task<AsyncLock> WaitForLockAsync(string lockName)
        {
            var l = _locks.GetOrAdd(lockName, x => new AsyncLock());
            await l.WaitAsync();
            return l;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var l in _locks.Values)
                    {
                        l?.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
