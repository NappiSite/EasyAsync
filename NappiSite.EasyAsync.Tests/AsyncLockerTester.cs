using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyAsync.Tests;

[TestClass]
public class AsyncLockerTest
{
    [TestMethod]
    public async Task GetLock_SameKey_SameLockAsync()
    {
        // Arrange
        using (var locker = new AsyncLocker())
        {
            var l1 = await locker.WaitForLockAsync("x");
            l1.Release();
            var l2 = await locker.WaitForLockAsync("x");

            // Assert
            Assert.AreSame(l1, l2);
        }
    }

    [TestMethod]
    public async Task GetLock_SameKey_DifferentLockers_DifferentLockAsync()
    {
        // Arrange
        using (var locker2 = new AsyncLocker())
        {
            using (var locker1 = new AsyncLocker())
            {
                var l1 = await locker1.WaitForLockAsync("x");
                var l2 = await locker2.WaitForLockAsync("x");

                // Assert
                Assert.AreNotSame(l1, l2);
            }
        }
    }

    [TestMethod]
    public async Task GetLock_SameKey_HonorLock()
    {
        // Arrange
        var lockIds = new List<string>();
        const int DEGREE_OF_PARALLELISM = 10;
        const int MILLISECONDS_DELAY = 100;
        for (var i = 0; i < DEGREE_OF_PARALLELISM; i++) lockIds.Add(i.ToString());
        var startTicks = DateTime.Now.Ticks;

        //Acct
        using (var locker = new AsyncLocker())
        {
            await lockIds.ForEachAsync(DEGREE_OF_PARALLELISM, async _ =>
            {
                var l1 = await locker.WaitForLockAsync("x");

                await Task.Delay(MILLISECONDS_DELAY);

                l1.Release();
            });
        }

        var duration = (DateTime.Now.Ticks - startTicks) / TimeSpan.TicksPerMillisecond;

        // Assert

        Assert.IsTrue(duration >= DEGREE_OF_PARALLELISM * MILLISECONDS_DELAY, duration.ToString());
    }

    [TestMethod]
    public async Task GetLock_DifferentKey_NonFactorLock()
    {
        // Arrange
        var lockIds = new List<string>();
        const int DEGREE_OF_PARALLELISM = 10;
        const int MILLISECONDS_DELAY = 100;
        for (var i = 0; i < DEGREE_OF_PARALLELISM; i++) lockIds.Add(i.ToString());
        var startTicks = DateTime.Now.Ticks;

        //Acct
        using (var locker = new AsyncLocker())
        {
            await lockIds.ForEachAsync(DEGREE_OF_PARALLELISM, async id =>
            {
                var l1 = await locker.WaitForLockAsync(id);

                await Task.Delay(MILLISECONDS_DELAY);

                l1.Release();
            });
        }

        var duration = (DateTime.Now.Ticks - startTicks) / TimeSpan.TicksPerMillisecond;

        // Assert

        Assert.IsTrue(duration < DEGREE_OF_PARALLELISM * MILLISECONDS_DELAY, duration.ToString());
    }


    [TestMethod]
    public async Task GetLock_LotsOfKeys_Works()
    {
        // Arrange
        var lockIds = new List<string>();
        const int DEGREE_OF_PARALLELISM = 100;
        const int MILLISECONDS_DELAY = 10;
        const int ELEMENTS = 10000;
        for (var i = 0; i < ELEMENTS; i++) lockIds.Add(i.ToString());
        var startTicks = DateTime.Now.Ticks;

        //Acct
        await Runalot(lockIds, DEGREE_OF_PARALLELISM, MILLISECONDS_DELAY);

        var duration = (DateTime.Now.Ticks - startTicks) / TimeSpan.TicksPerMillisecond;

        // Assert

        Assert.IsTrue(duration < ELEMENTS * MILLISECONDS_DELAY, duration.ToString());
    }


    private static async Task Runalot(List<string> lockIds, int degreeOfParallelism, int millisecondsDelay)
    {
        using (var locker = new AsyncLocker())
        {
            await lockIds.ForEachAsync(degreeOfParallelism, async id =>
            {
                var l1 = await locker.WaitForLockAsync(id);

                await Task.Delay(millisecondsDelay);

                l1.Release();
            });
        }
    }
}