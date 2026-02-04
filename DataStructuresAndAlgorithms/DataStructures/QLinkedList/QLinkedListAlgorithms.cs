using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loges.DataStructuresAndAlgorithms.DataStructures.QLinkedList {
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

    public void MergeSort() {
      if (head == null) {
        return;
      }
      MergeSortHelper(head, GetTail()!);
    }

    private void MergeSortHelper(QNode<T> head, QNode<T> tail) {
      if (head == tail) {
        return;
      }
      if (head.Next == tail) {
        Merge(head, head, tail, tail);
        return;
      }
      // Get halfway node
      QNode<T> fast = head;
      QNode<T> slow = head;
      while ((fast!.Next?.Next ?? tail) != tail) {
        fast = fast!.Next!.Next!;
        slow = slow.Next!;
      }
      MergeSortHelper(head, slow);
      MergeSortHelper(slow.Next!, tail);
      Merge(head, slow, slow.Next!, tail);
    }

    private void Merge(QNode<T> firstHead, QNode<T> firstTail, QNode<T> secondHead, QNode<T> secondTail) {
      QNode<T> originalHead = firstHead;
      QNode<T>? mergedList = null;
      QNode<T>? mergedListFollower = null;
      while (firstHead != firstTail.Next || secondHead != secondTail.Next) {
        T value;
        if (secondHead == secondTail.Next ||
          (firstHead != firstTail.Next && Comparer<T>.Default.Compare(firstHead.Value, secondHead.Value) <= 0)) {
          value = firstHead.Value;
          firstHead = firstHead.Next;
        } else {
          value = secondHead.Value;
          secondHead = secondHead.Next;
        }
        if (mergedList == null) {
          mergedList = new QNode<T> {
            Value = value,
            Next = null
          };
          mergedListFollower = mergedList;
        } else {
          mergedListFollower!.Next = new QNode<T> {
            Value = value,
            Next = null
          };
          mergedListFollower = mergedListFollower.Next;
        }
      }
      // Copy values back into original
      while (mergedList != null) {
        originalHead!.Value = mergedList.Value;
        originalHead = originalHead!.Next!;
        mergedList = mergedList.Next;
      }
    }
  }
}
