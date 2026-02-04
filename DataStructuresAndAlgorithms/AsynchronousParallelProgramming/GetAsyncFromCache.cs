using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.AsynchronousParallelProgramming {
  internal class GetAsyncFromCache<TKey, TValue> where TKey : notnull {
    private readonly ConcurrentDictionary<TKey, Lazy<Task<TValue>>> _map
      = new ConcurrentDictionary<TKey, Lazy<Task<TValue>>>();

    public Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory) {
      if (factory is null) {
        throw new ArgumentNullException(nameof(factory));
      }

      Lazy<Task<TValue>> lazy = _map.GetOrAdd(key, new Lazy<Task<TValue>>(() => factory(key), LazyThreadSafetyMode.ExecutionAndPublication));

      Task<TValue> task;
      try {
        // Might throw if factory throws synchronusly.
        task = lazy.Value;
      } catch {
        // Evict and rethrow.
        _map.TryRemove(new KeyValuePair<TKey, Lazy<Task<TValue>>>(key, lazy));
        throw;
      }

      if (!task.IsCompleted) {
        task.ContinueWith(t => {
          if (t.IsFaulted || t.IsCanceled) {
            _map.TryRemove(new KeyValuePair<TKey, Lazy<Task<TValue>>>(key, lazy));
          }
        },
          CancellationToken.None,
          TaskContinuationOptions.ExecuteSynchronously,
          TaskScheduler.Default
        );
      } else if (task.IsFaulted || task.IsCanceled) {
        _map.TryRemove(new KeyValuePair<TKey, Lazy<Task<TValue>>>(key, lazy));
      }
      return task;
    }
  }
}
