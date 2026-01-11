using DataStructuresAndAlgorithms.DataStructures;

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

      Console.ReadKey();
    }
  }
}
