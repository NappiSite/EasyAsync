using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NappiSite.EasyAsync.Tests;

[TestClass]
public class AsyncHelperTester
{
    private const int MAX_LOOP = 500;
    private const int DEGREES_OF_PARALLELISM = 100;
    private const int DELAY = 100;

    [TestMethod]
    public async Task ForEachAsync_Completes()
    {
        // Arrange
        var list = new List<int>();
        for (var i = 0; i < 10; i++) list.Add(DELAY);

        var output = new ConcurrentBag<int>();

        // Act
        await list.ForEachAsync(async i =>
        {
            await Task.Delay(DELAY);
            output.Add(i);
        });

        // Assert
        Assert.AreEqual(list.Sum(), output.Sum());
    }

    [TestMethod]
    public async Task ParallelForEachAsync_WithParallelismDefaultToProcessorCount_Completes()
    {
        // Arrange
        var list = new List<int>();
        for (var i = 0; i < MAX_LOOP; i++) list.Add(DELAY);

        var output = new ConcurrentBag<int>();

        // Act
        await list.ParallelForEachAsync(async i =>
        {
            await Task.Delay(DELAY);
            output.Add(i);
        });

        // Assert
        Assert.AreEqual(list.Sum(), output.Sum());
    }

    [TestMethod]
    public async Task ParallelForEachAsync_WithParallelismSpecified_Completes()
    {
        // Arrange
        var list = new List<int>();
        for (var i = 0; i < MAX_LOOP; i++) list.Add(DELAY);

        var output = new ConcurrentBag<int>();

        // Act
        await list.ParallelForEachAsync(async i =>
        {
            await Task.Delay(DELAY);
            output.Add(i);
        },DEGREES_OF_PARALLELISM);

        // Assert
        Assert.AreEqual(list.Sum(), output.Sum());
    }
}