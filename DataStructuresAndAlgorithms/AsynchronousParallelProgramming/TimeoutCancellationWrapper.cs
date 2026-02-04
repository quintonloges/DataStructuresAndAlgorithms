using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.AsynchronousParallelProgramming {
  internal class TimeoutCancellationWrapper {
    public async Task<T> WithTimeout<T>(Task<T> task, TimeSpan timeout, CancellationToken ct) {
      ct.ThrowIfCancellationRequested();

      using CancellationTokenSource delayCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
      Task delayTask = Task.Delay(timeout, delayCts.Token);

      Task winner = await Task.WhenAny(task, delayTask);

      if (winner == delayTask) {
        ct.ThrowIfCancellationRequested();
        throw new TimeoutException();
      }

      delayCts.Cancel();

      return await task;
    }
  }
}
