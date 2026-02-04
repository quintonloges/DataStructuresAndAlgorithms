using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.AsynchronousParallelProgramming {
  internal class ProducerConsumerQueue<T> {
    private readonly Queue<T> ready = new Queue<T>();
    private readonly Queue<Waiter> waiting = new Queue<Waiter>();
    private bool isCompleted = false;
    private Exception? providedException = null;

    private sealed class Waiter {
      public TaskCompletionSource<T> Tcs { get; }
      public CancellationTokenRegistration Ctr { get; set; }

      public Waiter(TaskCompletionSource<T> tcs) => Tcs = tcs;
    }

    public void Enqueue(T item) {
      TaskCompletionSource<T>? tcs = null;
      lock (ready) {
        if (isCompleted) {
          throw providedException ?? new InvalidOperationException("Cannot enqueue items to completed queue.");
        }
        while (waiting.Count > 0) {
          Waiter waiter = waiting.Dequeue();
          if (waiter.Tcs.Task.IsCanceled) {
            waiter.Ctr.Dispose();
            continue;
          }
          tcs = waiter.Tcs;
          waiter.Ctr.Dispose();
          break;
        }
        if (tcs is null) {
          ready.Enqueue(item);
        }
      }
      tcs?.TrySetResult(item);
    }

    public Task<T> DequeueAsync(CancellationToken ct) {
      lock (ready) {
        ct.ThrowIfCancellationRequested();
        if (ready.Count > 0) {
          return Task.FromResult(ready.Dequeue());
        }
        if (isCompleted) {
          return Task.FromException<T>(providedException ?? new InvalidOperationException("Cannot dequeue from a completed empty queue."));
        }
        TaskCompletionSource<T> iou = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        Waiter waiter = new Waiter(iou);
        if (ct.CanBeCanceled) {
          waiter.Ctr = ct.Register(() => iou.TrySetCanceled());
        }
        waiting.Enqueue(waiter);
        return iou.Task;
      }
    }

    public void Complete(Exception? error = null) {
      List<Waiter>? waiters = null;
      lock (ready) {
        if (isCompleted) {
          return;
        }

        isCompleted = true;
        providedException = error;

        if (ready.Count == 0 && waiting.Count > 0) {
          waiters = new List<Waiter>(waiting.Count);
          while (waiting.Count > 0) {
            waiters.Add(waiting.Dequeue());
          }
        }
      }
      if (waiters is null) {
        return;
      }
      Exception ex = providedException ?? new InvalidOperationException("Queue has completed with no elements left to wait for.");
      foreach (Waiter waiter in waiters) {
        waiter.Ctr.Dispose();
        waiter.Tcs.TrySetException(ex);
      }
    }
  }
}
