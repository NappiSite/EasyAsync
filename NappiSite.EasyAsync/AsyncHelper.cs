using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace NappiSite.EasyAsync
{
    public static class AsyncHelper
    {
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, int degreeOfParallelism, Func<T, Task> body)
        {
            using (var semaphore = new SemaphoreSlim(degreeOfParallelism, degreeOfParallelism))
            {
                var tasks = source.Select(async item =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await body(item);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
                await Task.WhenAll(tasks);
            }
        }
    }
}
