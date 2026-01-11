using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresAndAlgorithms.DataStructures {
  internal class QLinkedList<T> {
    private QNode<T>? head;

    /// <summary>
    /// Pushes new data to the front of the list.
    /// </summary>
    /// <param name="data">Data to store.</param>
    public void Push(T data) {
      QNode<T> newNode = new QNode<T> {
        Value = data,
        Next = head
      };
      head = newNode;
    }

    /// <summary>
    /// Searches the list for the specified data.
    /// </summary>
    /// <param name="data">Value of QNode to retrieve.</param>
    /// <returns>QNode containing the value if found, null if not.</returns>
    public QNode<T>? GetNode(T data) {
      QNode<T>? cur = head;
      while (cur != null) {
        if (cur.Value!.Equals(data)) {
          return cur;
        }
        cur = cur.Next;
      }
      return null;
    }

    /// <summary>
    /// Find whether the given data exists in the list.
    /// </summary>
    /// <param name="data">Value to find.</param>
    /// <returns>True if data exists, false if not</returns>
    public bool Contains(T data) {
      return GetNode(data) != null;
    }

    /// <summary>
    /// Removes the first instance of the found value.
    /// </summary>
    /// <param name="data">Value to remove.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the value is not found in the list.</exception>
    public void RemoveValue(T data) {
      QNode<T>? prev = null;
      QNode<T>? cur = head;
      while (cur != null) {
        if (cur.Value!.Equals(data)) {
          break;
        }
        prev = cur;
        cur = cur.Next;
      }
      if (cur == null) {
        throw new KeyNotFoundException("The value specified in the argument could not be found.");
      }
      if (prev == null) {
        head = null;
        return;
      }
      prev.Next = cur.Next;
    }

    /// <summary>
    /// Pops the value at the beginning of the List.
    /// </summary>
    /// <returns>QNode from the beginning of the list.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the collection is empty.</exception>
    public QNode<T> Pop() {
      QNode<T>? cur = head;
      if (head == null) {
        throw new IndexOutOfRangeException("Cannot pop value out of empty list.");
      }
      head = head.Next;
      return cur!;
    }

    /// <summary>
    /// Returns the QNode at the beginning of the list.
    /// </summary>
    /// <returns>QNode at the beginning of the list.</returns>
    public QNode<T>? Peek() {
      return head;
    }

    /// <summary>
    /// Reverse the list.
    /// </summary>
    public void Reverse() {
      QNode<T>? cur = head;
      QNode<T>? curHead = null;
      while (cur != null) {
        QNode<T>? next = cur.Next;
        cur.Next = curHead;
        curHead = cur;
        head = curHead;
        cur = next;
      }
    }

    public override string ToString() {
      QNode<T>? cur = head;
      string ret = "[";
      while (cur != null) {
        ret += cur.ToString();
        if (cur.Next != null) {
          ret += ", ";
        }
        cur = cur.Next;
      }
      ret += "]";
      return ret;
    }
  }

  internal class QNode<T> {
    public required T Value;
    public QNode<T>? Next;

    public override string ToString() {
      return Value!.ToString();
    }
  }
}
