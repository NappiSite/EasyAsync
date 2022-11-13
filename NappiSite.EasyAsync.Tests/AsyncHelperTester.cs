using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;

namespace NappiSite.EasyAsync.Tests
{
    [TestClass]
    public class AsyncHelperTester
    {
        private const int MAX_LOOP = 500;
        private const int DEGREES_OF_PARALLELISM = 100;
        private const int DELAY = 100;

        [TestMethod]        
        public async Task ParallelForEachSemaphoreAsync_Completes()
        {
            // Arrange
            var list = new List<int>();
            for (var i = 0; i < MAX_LOOP; i++)
            {
                list.Add(DELAY);
            }

            var output = new ConcurrentBag<int>();

            // Act
            await list.ParallelForEachAsync(DEGREES_OF_PARALLELISM, async i =>
            {
                await Task.Delay(DELAY);
                output.Add(i);
            });

            // Assert
            Assert.AreEqual(list.Sum(), output.Sum());
        }
    }
}