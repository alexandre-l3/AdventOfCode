namespace AdventOfCode._2025;

public sealed class Day11 : IDay
{
    public string SolvePartOne()
    {
        var neighbours = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.Split(':'))
            .ToDictionary(k => k[0], v => v[1].Trim().Split(' '));

        var paths = 0;
        var stack = new Stack<string>(["you"]);
        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (node == "out")
            {
                paths++;
            }

            var nextNodes = neighbours.GetValueOrDefault(node, []);

            foreach (var next in nextNodes)
            {
                stack.Push(next);
            }
        }

        return paths.ToString();
    }

    public string SolvePartTwo()
    {
        var neighbours = File.ReadAllLines("2025/input1.txt")
            .Select(x => x.Split(':'))
            .ToDictionary(k => k[0], v => v[1].Trim().Split(' '));

        var path = new[] { "svr", "fft", "dac", "out" };

        return path.SkipLast(1)
            .Select((x, i) => Dfs(x, path[i + 1], neighbours, []))
            .Aggregate(1L, (x, y) => x * y, z => z.ToString());
    }

    private static long Dfs(
        string from,
        string to,
        Dictionary<string, string[]> neighbours,
        Dictionary<string, long> cache)
    {
        if (from == to)
        {
            return 1;
        }

        if (cache.TryGetValue(from, out var value))
        {
            return value;
        }

        return cache[from] = neighbours.GetValueOrDefault(from, [])
            .Sum(next => Dfs(next, to, neighbours, cache));
    }
}