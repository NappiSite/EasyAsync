# EasyAsync
EasyAsync contains convenience methods to make it easier to deal with async code, and provides async behavior to older .NET versions.

* ForEachAsync : ForEachAsync is based on "[Implementing a simple ForEachAsync](https://devblogs.microsoft.com/pfxteam/implementing-a-simple-foreachasync/)", and uses a SemaphoreSlim to provide a throttling mechanism for Task.WhenAll.
* AsynLocker : Also utilized SemaphoreSlim, to provide a named async lock mechanism.

## Examples
### ForEachAsync
```csharp
var mdp = 2;
var someList = new List<int>();
...

await someList.ForEachAsync(mdp, async i=>{
    await DoWorkAsync();
    ...
});
```
### AsyncLocker
```csharp
using (var locker = new AsyncLocker())
{
    await lockIds.ForEachAsync(DEGREE_OF_PARALLELISM, async _ =>
    {
        var lockName = "lockX"
        var lck = await locker.WaitForLockAsync(lockName);

        await DoWorkAsync();

        lck.Release();
    });
}
```



