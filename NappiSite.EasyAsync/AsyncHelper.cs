using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NappiSite.EasyAsync
{
    public static class AsyncHelper
    {
        [Obsolete("If you're looking for parallel foreachasync use ParallelForEachAsync ")]
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int degreeOfParallelism,
            Func<T, Task> body)
        {
            return ParallelForEachAsync(source,  body,degreeOfParallelism);
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> source, 
            Func<T, Task> body)
        {
            return ParallelForEachAsync(source,  body,1);
        }

        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, 
            Func<T, Task> body)
        {
            var degreeOfParallelism=Environment.ProcessorCount;
            return ParallelForEachAsync(source,  body,degreeOfParallelism);
        }

        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, 
            Func<T, Task> body,int degreeOfParallelism)
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