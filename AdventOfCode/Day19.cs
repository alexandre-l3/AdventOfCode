using System.Numerics;

namespace AdventOfCode;

public sealed class Day19 : IDay
{
    public string SolvePartOne() => _designs
        .Count(design => IsPossibleDesign(design.Length - 1, design, new Dictionary<int, bool>(), _patterns))
        .ToString();

    public string SolvePartTwo() => _designs
        .Sum(design => CountNumberOfWays(design, new Dictionary<string, long>(), _patterns))
        .ToString();

    private static bool IsPossibleDesign(int index, string target, Dictionary<int , bool> cache, string[] patterns)
    {
        if (index < 0)
        {
            return true;
        }

        if (cache.TryGetValue(index, out var value))
        {
            return value;
        }

        foreach (var pattern in patterns)
        {
            if (index - pattern.Length + 1 < 0)
            {
                continue;
            }

            if (target.Substring(index - pattern.Length + 1, pattern.Length) == pattern &&
                IsPossibleDesign(index - pattern.Length, target, cache, patterns))
            {
                cache[index] = true;
                return true;
            }
        }

        cache[index] = false;
        return false;
    }
    
    private static long CountNumberOfWays(string target, Dictionary<string, long> cache, string[] patterns)
    {
        var count = 0L;
        foreach (var pattern in patterns.Where(target.StartsWith))
        {
            if (pattern.Length == target.Length)
            {
                count++;
            }
            else
            {
                var substring = target[pattern.Length..];
                if (!cache.ContainsKey(substring))
                {
                    cache[substring] = CountNumberOfWays(substring, cache, patterns);
                }
                count += cache[substring];
            }
        }

        return count;
    }

    private readonly string[] _patterns = "r, wr, b, g, bwu, rb, gb, br".Split(", ").ToArray();

    private readonly string[] _designs =
    [
        "brwrr",
        "bggr",
        "gbbr",
        "rrbgbr",
        "ubwu",
        "bwurrg",
        "brgr",
        "bbrgwb"
    ];
}