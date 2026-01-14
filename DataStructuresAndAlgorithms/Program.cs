using DataStructuresAndAlgorithms.DataStructures.QLinkedList;

namespace DataStructuresAndAlgorithms {
  internal class Program {
    static void Main(string[] args) {
      // 100% coverage of QLinkedList
      QLinkedList<string> testList = new QLinkedList<string>();
      testList.Push("First");
      testList.Push("Middle");
      testList.Push("Last");

      Console.WriteLine("Data has been insterted in the linked list.");
      Console.WriteLine($"Data: {testList.ToString()}");

      Console.WriteLine("Reversing the list...");
      testList.Reverse();
      Console.WriteLine($"Data: {testList.ToString()}");

      Console.WriteLine($"Contains value 'Last': {testList.Contains("Last")}");
      Console.WriteLine($"Contains value 'Middle': {testList.Contains("Middle")}");
      Console.WriteLine($"Contains value 'First': {testList.Contains("First")}");
      Console.WriteLine($"Contains value 'Second': {testList.Contains("Second")}");

      Console.WriteLine("Removing 'Middle'...");
      testList.RemoveValue("Middle");
      Console.WriteLine($"Data: {testList.ToString()}");
      Console.WriteLine($"Contains value 'Middle': {testList.Contains("Middle")}");

      Console.WriteLine("Finding node 'Last'...");
      QNode<string>? last = testList.GetNode("Last");
      Console.WriteLine($"Value of found node: {last?.Value ?? "Not found"}");

      Console.WriteLine("Popping node...");
      QNode<string>? popped = testList.Pop();
      Console.WriteLine($"Value of popped node: {popped?.Value ?? "Not found"}");
      Console.WriteLine($"Data: {testList.ToString()}");

      Console.WriteLine("Peeking node...");
      QNode<string>? peeked = testList.Peek();
      Console.WriteLine($"Value of peeked node: {peeked?.Value ?? "Not found"}");
      Console.WriteLine($"Data: {testList.ToString()}");

      Console.WriteLine("Evaluating List Sorting...");
      QLinkedList<int> listSortTester = new QLinkedList<int>();
      listSortTester.Push(5);
      listSortTester.Push(2);
      listSortTester.Push(9);
      listSortTester.Push(12);
      listSortTester.Push(6);
      listSortTester.Push(4);
      listSortTester.Push(7);
      Console.WriteLine($"Current Data: {listSortTester.ToString()}");
      listSortTester.MergeSort();
      Console.WriteLine($"After MergeSort Data: {listSortTester.ToString()}");
      listSortTester.Reverse();
      Console.WriteLine($"Reversing Data: {listSortTester.ToString()}");
      listSortTester.QuickSort();
      Console.WriteLine($"After QuickSort Data: {listSortTester.ToString()}");
      QLinkedList<int> newPartition = listSortTester.Partition(2, 2);
      Console.WriteLine($"New Partition: {newPartition.ToString()}");
      Console.WriteLine($"Previous Partition: {listSortTester.ToString()}");


      Console.ReadKey();
    }
  }
}
