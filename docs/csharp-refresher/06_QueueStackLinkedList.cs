// ============================================================
// Queue<T>   — FIFO  (first-in, first-out)
// Stack<T>   — LIFO  (last-in, first-out)
// LinkedList<T> — doubly-linked list; O(1) insert/remove at
//                 any node when you already hold the node ref
// ============================================================

using System;
using System.Collections.Generic;

namespace CollectionsRefresher;

public static class QueueStackLinkedListExamples
{
    public static void Run()
    {
        // ---- Queue<T> ----
        var queue = new Queue<string>();
        queue.Enqueue("first");
        queue.Enqueue("second");
        queue.Enqueue("third");

        string next    = queue.Peek();      // "first" — does not remove
        string removed = queue.Dequeue();   // "first" — removes it

        queue.TryDequeue(out string? safe); // safe version (no exception on empty)
        queue.TryPeek(out string? peeked);

        Console.WriteLine($"Queue count: {queue.Count}");

        // ---- Stack<T> ----
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        int top     = stack.Peek();   // 3
        int popped  = stack.Pop();    // 3

        stack.TryPop(out int safePop);
        stack.TryPeek(out int safePeek);

        // Iterating a stack yields top-to-bottom
        foreach (int n in stack)
            Console.WriteLine(n);  // 2, then 1

        // ---- LinkedList<T> ----
        var list = new LinkedList<string>();

        LinkedListNode<string> nodeB = list.AddFirst("B");
        LinkedListNode<string> nodeA = list.AddBefore(nodeB, "A");   // A <-> B
        LinkedListNode<string> nodeC = list.AddAfter(nodeB, "C");    // A <-> B <-> C
        list.AddLast("D");                                            // A <-> B <-> C <-> D

        // Remove by node reference — O(1)
        list.Remove(nodeB);   // A <-> C <-> D

        // Access ends
        string? head = list.First?.Value;   // "A"
        string? tail = list.Last?.Value;    // "D"

        // Forward traversal
        LinkedListNode<string>? node = list.First;
        while (node is not null)
        {
            Console.Write(node.Value + " ");
            node = node.Next;
        }

        // Find
        LinkedListNode<string>? found = list.Find("C");

        Console.WriteLine($"\nLinkedList count: {list.Count}");
    }
}
