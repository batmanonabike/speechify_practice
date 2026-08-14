// ============================================================
// Dictionary<TKey, TValue>  —  hash map / associative array
// ============================================================
// O(1) average lookup, insert, and delete by key.
// Keys must be unique; values may repeat.
// Underlying structure: hash table with open addressing buckets.
// ============================================================

using System;
using System.Collections.Generic;

namespace CollectionsRefresher;

public static class DictionaryExamples
{
    public static void Run()
    {
        // --- Construction ---
        var scores = new Dictionary<string, int>
        {
            ["Alice"] = 95,
            ["Bob"]   = 82,
            ["Carol"] = 91,
        };

        // --- Add / update ---
        scores["Dave"]  = 78;           // add new key
        scores["Alice"] = 97;           // update existing key (no exception)

        scores.TryAdd("Eve", 88);       // safe add — does nothing if key exists

        // --- Remove ---
        scores.Remove("Dave");
        scores.Remove("Bob", out int bobScore);   // also retrieves the value

        // --- Lookup ---
        int aliceScore = scores["Alice"];          // throws KeyNotFoundException if missing

        // Safe lookup patterns
        if (scores.TryGetValue("Carol", out int carolScore))
            Console.WriteLine($"Carol: {carolScore}");

        int eveScore = scores.GetValueOrDefault("Eve", defaultValue: 0);

        // --- ContainsKey / ContainsValue ---
        bool hasAlice = scores.ContainsKey("Alice");
        bool has95    = scores.ContainsValue(95);

        // --- Iteration ---
        foreach (KeyValuePair<string, int> kv in scores)
            Console.WriteLine($"  {kv.Key} => {kv.Value}");

        // Deconstruct syntax (C# 7+)
        foreach (var (name, score) in scores)
            Console.WriteLine($"  {name}: {score}");

        // Keys / Values collections
        IEnumerable<string> names   = scores.Keys;
        IEnumerable<int>    values  = scores.Values;

        // --- Merge / upsert pattern ---
        var extra = new Dictionary<string, int> { ["Frank"] = 70, ["Alice"] = 99 };
        foreach (var (k, v) in extra)
            scores[k] = v;   // last-writer-wins upsert

        // --- Case-insensitive keys ---
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = "application/json"
        };
        bool found = headers.ContainsKey("content-type");   // true

        // --- Nested dictionary ---
        var graph = new Dictionary<string, List<string>>
        {
            ["A"] = ["B", "C"],
            ["B"] = ["D"],
        };
        graph.TryGetValue("A", out var neighbors);

        Console.WriteLine($"Count: {scores.Count}");
    }
}
