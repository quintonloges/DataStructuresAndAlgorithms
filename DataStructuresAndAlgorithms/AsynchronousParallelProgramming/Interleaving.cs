using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.AsynchronousParallelProgramming {
  internal class Interleaving<TResult> {
    public async IAsyncEnumerable<TResult> InCompletionOrder<TResult>(IEnumerable<Task<TResult>> tasks, CancellationToken ct) {
      List<Task<TResult>> taskList = tasks.ToList();
      if (taskList.Count == 0) {
        yield break;
      }
      TaskCompletionSource<TResult>[] completionSources = Enumerable.Range(0, taskList.Count)
        .Select((_) => new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously))
        .ToArray();

      int index = -1;
      foreach (Task<TResult> completedTask in taskList) {
        completedTask.ContinueWith((t) => {
          TaskCompletionSource<TResult> completionSource = completionSources[Interlocked.Increment(ref index)];
          if (t.IsCanceled) {
            completionSource.TrySetCanceled();
          } else if (t.IsFaulted) {
            completionSource.TrySetException(t.Exception.InnerExceptions);
          } else {
            completionSource.TrySetResult(t.Result);
          }
        },
          CancellationToken.None,
          TaskContinuationOptions.ExecuteSynchronously,
          TaskScheduler.Default
        );
      }
      foreach (TaskCompletionSource<TResult> completion in completionSources) {
        ct.ThrowIfCancellationRequested();
        yield return await completion.Task;
      }
    }
  }
}
