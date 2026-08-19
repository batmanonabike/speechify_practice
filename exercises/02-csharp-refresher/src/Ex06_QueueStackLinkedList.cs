// Exercise 06 - Queue<T>, Stack<T>, LinkedList<T>
// Reference: docs/csharp-refresher/06_QueueStackLinkedList.cs

using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace CSharpExercises;

public static class QueueStackExercises
{
    /// <summary>
    /// Given a string, use a Stack to reverse it character by character.
    /// Hint: push each char onto a stack, then pop into a new string.
    /// </summary>
    public static string ReverseString(string input)
    {
        var sb = new StringBuilder();
        var stack = new Stack<char>();

        foreach (var c in input)
            stack.Push(c);

        while (stack.TryPop(out var result))
            sb.Append(result);

        return sb.ToString();
    }

    /// <summary>
    /// Return true if <paramref name="input"/> is a palindrome,
    /// using a Queue and a Stack to compare front-to-back vs back-to-front.
    /// Ignore case. e.g. "racecar" → true, "hello" → false.
    /// Hint: enqueue all chars, push all chars; dequeue and pop must match.
    /// </summary>
    public static bool IsPalindrome(string input)
    {
        var queue = new Queue<char>(input.Length);
        var stack = new Stack<char>(input.Length);

        foreach (char c in input)
        {
            var l = char.ToLower(c);
            queue.Enqueue(l);
            stack.Push(l);
        }

        while (stack.TryPop(out var a))
        {
            var b = queue.Dequeue();
            if (!a.Equals(b))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Simulate a simple task scheduler: given a Queue of task names, process
    /// them FIFO and return the order in which they were processed.
    /// </summary>
    public static List<string> ProcessQueue(IEnumerable<string> tasks)
    {
        var result = new List<string>();
        var queue = new Queue<string>(tasks);

        Action<string> simulateExecuteTask = (x) => { };

        while (queue.TryDequeue(out var item))
        {
            simulateExecuteTask(item);
            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Given a LinkedList<int>, remove all nodes whose value is negative,
    /// and return the modified list.
    /// Hint: iterate with LinkedListNode<T>, use Remove(node).
    /// </summary>
    public static LinkedList<int> RemoveNegatives(LinkedList<int> list)
    {
        var node = list.First;
        while (node != null)
        {
            var nextNode = node.Next;
            if (node.Value < 0)
                list.Remove(node);
            node = nextNode;
        }
        return list;
    }
}
