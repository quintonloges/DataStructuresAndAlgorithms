using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.AsynchronousParallelProgramming {
  internal class ThreadSafeAggregator {
    public Task<IDictionary<string, int>> CountWordsParallelAsyncV1(IEnumerable<string> lines, int maxConcurrency, CancellationToken ct) {
      return Task.Run(() => {
        ConcurrentDictionary<string, int> map = new ConcurrentDictionary<string, int>();

        ParallelOptions options = new ParallelOptions {
          MaxDegreeOfParallelism = maxConcurrency,
          CancellationToken = ct
        };

        Parallel.ForEach(lines,
          options,
          () => new Dictionary<string, int>(),
          (string line, ParallelLoopState loop, Dictionary<string, int> threadMap) => {
            string[] words = line.Split(" ");
            foreach (string word in words) {
              if (threadMap.ContainsKey(word)) {
                threadMap[word]++;
              } else {
                threadMap.Add(word, 1);
              }
            }
            return threadMap;
          },
          (finalResult) => {
            foreach (KeyValuePair<string, int> kvp in finalResult) {
              map.AddOrUpdate(kvp.Key, kvp.Value, (key, val) => val + kvp.Value);
            }
          }
        );
        return (IDictionary<string, int>)map;
      }, ct);
    }

    public async Task<IDictionary<string, int>> CountWordsParallelAsyncV2(IEnumerable<string> lines, int maxConcurrency, CancellationToken ct) {
      ConcurrentDictionary<string, int> map = new ConcurrentDictionary<string, int>();

      using SemaphoreSlim sem = new SemaphoreSlim(maxConcurrency);

      IEnumerable<Task> tasks = lines.Select(async (line) => {
        await sem.WaitAsync(ct);

        try {
          foreach (string word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
            map.AddOrUpdate(word, 1, (_, v) => v + 1);
          }
        } finally {
          sem.Release();
        }
      });

      await Task.WhenAll(tasks);
      return map;
    }

    public Task<IDictionary<string, int>> CountWordsParallelAsnycV3(IEnumerable<string> lines, int maxConcurrency, CancellationToken ct) {
      return Task.Run(() => (IDictionary<string, int>)lines
        .AsParallel()
        .WithDegreeOfParallelism(maxConcurrency)
        .WithCancellation(ct)
        .SelectMany(s => s.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        .GroupBy(s => s)
        .ToDictionary(c => c.Key, c => c.Count()), ct);
    }

    public async Task<IDictionary<string, int>> CountWordsParallelAsyncV4(IEnumerable<string> lines, int maxConcurrency, CancellationToken ct) {
      Dictionary<string, int> map = new Dictionary<string, int>();

      ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

      using SemaphoreSlim sem = new SemaphoreSlim(maxConcurrency, maxConcurrency);

      IEnumerable<Task> tasks = lines
        .Select(async (line) => {
          await sem.WaitAsync(ct);
          try {
            ct.ThrowIfCancellationRequested();

            Dictionary<string, int> localDict = new Dictionary<string, int>();
            foreach (string s in line.Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
              localDict[s] = localDict.GetValueOrDefault(s, 0) + 1;
            }

            rwLock.EnterWriteLock();
            try {
              foreach (KeyValuePair<string, int> kvp in localDict) {
                map[kvp.Key] = map.GetValueOrDefault(kvp.Key, 0) + kvp.Value;
              }
            } finally {
              rwLock.ExitWriteLock();
            }
          } finally {
            sem.Release();
          }
        });
      try {
        await Task.WhenAll(tasks);
        return map;
      } finally {
        rwLock.Dispose();
      }
    }
  }
}
