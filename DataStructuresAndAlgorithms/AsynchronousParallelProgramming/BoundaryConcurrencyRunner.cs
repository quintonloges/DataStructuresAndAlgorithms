using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.AsynchronousParallelProgramming {
  internal class BoundaryConcurrencyRunner {
    public async Task<IReadOnlyList<TResult>> RunBoundedAsync<T, TResult>(IEnumerable<T> items, int maxConcurrency, Func<T, CancellationToken, Task<TResult>> work, CancellationToken ct) {
      List<T> itemList = items.ToList();
      TResult[] ret = new TResult[itemList.Count];

      using SemaphoreSlim sem = new SemaphoreSlim(maxConcurrency, maxConcurrency);

      Task[] tasks = itemList.Select(async (item, index) => {
        await sem.WaitAsync(ct);
        try {
          ret[index] = await work(item, ct);
        } finally {
          sem.Release();
        }
      }).ToArray();
      await Task.WhenAll(tasks);
      return ret;
    }
  }
}
