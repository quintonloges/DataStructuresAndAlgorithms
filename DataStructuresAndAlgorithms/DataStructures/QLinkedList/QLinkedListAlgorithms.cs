using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresAndAlgorithms.DataStructures.QLinkedList {
  internal partial class QLinkedList<T> {
    public void QuickSort() {
      QNode<T>? tail = GetTail();
      QuickSortHelper(head, tail);
    }

    private void QuickSortHelper(QNode<T>? head, QNode<T>? tail) {
      if (head == null || tail == null || head == tail) {
        return;
      }

      QNode<T>? pivot = QuickSortPartition(head, tail);

      QuickSortHelper(head, pivot);
      QuickSortHelper(pivot.Next, tail);
    }

    private QNode<T> QuickSortPartition(QNode<T> head, QNode<T> tail) {
      // Choose head as pivot
      QNode<T> pivot = head;

      QNode<T>? cur = head;
      QNode<T>? pre = head;

      while (cur != tail.Next) {
        if (Comparer<T>.Default.Compare(cur.Value, pivot.Value) < 0) {
          (cur.Value, pre.Next.Value) = (pre.Next.Value, cur.Value);
          pre = pre.Next;
        }
        cur = cur.Next;
      }

      (pre.Value, pivot.Value) = (pivot.Value, pre.Value);

      return pre;
    }
  }
}
