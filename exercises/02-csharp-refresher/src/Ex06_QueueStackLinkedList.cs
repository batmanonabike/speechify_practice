// Exercise 06 - Queue<T>, Stack<T>, LinkedList<T>
// Reference: docs/csharp-refresher/06_QueueStackLinkedList.cs

namespace CSharpExercises;

public static class QueueStackExercises
{
    /// <summary>
    /// Given a string, use a Stack to reverse it character by character.
    /// Hint: push each char onto a stack, then pop into a new string.
    /// </summary>
    public static string ReverseString(string input)
        => throw new NotImplementedException();

    /// <summary>
    /// Return true if <paramref name="input"/> is a palindrome,
    /// using a Queue and a Stack to compare front-to-back vs back-to-front.
    /// Ignore case. e.g. "racecar" → true, "hello" → false.
    /// Hint: enqueue all chars, push all chars; dequeue and pop must match.
    /// </summary>
    public static bool IsPalindrome(string input)
        => throw new NotImplementedException();

    /// <summary>
    /// Simulate a simple task scheduler: given a Queue of task names, process
    /// them FIFO and return the order in which they were processed.
    /// </summary>
    public static List<string> ProcessQueue(IEnumerable<string> tasks)
        => throw new NotImplementedException();

    /// <summary>
    /// Given a LinkedList<int>, remove all nodes whose value is negative,
    /// and return the modified list.
    /// Hint: iterate with LinkedListNode<T>, use Remove(node).
    /// </summary>
    public static LinkedList<int> RemoveNegatives(LinkedList<int> list)
        => throw new NotImplementedException();
}
