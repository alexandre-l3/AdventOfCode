namespace AdventOfCode._2025;

public sealed class Day10 : IDay
{
    public string SolvePartOne()
    {

        using var stream = new StreamReader("2025/input1.txt");

        var result = 0;
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!
                .Split(' ');
            var target = line[0][1..^1];

            var presses = line.Skip(1).SkipLast(1)
                .Select(x => x.Replace("(", "").Replace(")", ""))
                .Select(x => x.Split(',').Select(int.Parse).ToArray())
                .ToArray();

            var start = new string(Enumerable.Range(0, target.Length).Select(_ => '.')
                .ToArray());
            var queue = new Queue<string>([start]);

            var level = 0;
            var seen = new HashSet<string>([start]);
            var min = int.MaxValue;
            while (queue.Count > 0)
            {
                var k = queue.Count;
                for (var i = 0; i < k; i++)
                {
                    var node = queue.Dequeue();
                    if (target == node)
                    {
                        min = level;
                        break;
                    }

                    foreach (var press in presses)
                    {
                        var next = node.ToCharArray();
                        foreach (var button in press)
                        {
                            next[button] = node[button] == '.' ? '#' : '.';
                        }
                        var str = new string(next);
                        if (seen.Add(str))
                        {
                            queue.Enqueue(str);
                        }
                    }
                }

                level++;
            }

            result += min;
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        using var stream = new StreamReader("2025/sample.txt");

        var result = 0;
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!
                .Split(' ');
            var target = line.Last()[1..^1];
            var n = target.Split(',').Length;

            var presses = line.Skip(1).SkipLast(1)
                .Select(x => x.Replace("(", "").Replace(")", ""))
                .Select(x => x.Split(',').Select(y => int.Parse(y)).ToArray())
                .ToArray();

            var start = new int[n];
            var queue = new Queue<int[]>([start]);

            var level = 0;
            var seen = new HashSet<string>([string.Join(",", start)]);
            var min = int.MaxValue;
            while (queue.Any())
            {
                var k = queue.Count;
                for (var i = 0; i < k; i++)
                {
                    var node = queue.Dequeue();
                    if (target == string.Join(",", node))
                    {
                        min = Math.Min(level, min);
                        break;
                    }

                    foreach (var press in presses)
                    {
                        var next = node.ToArray();
                        foreach (var button in press)
                        {
                            next[button]++;
                        }
                        var str = new string(string.Join(",", next));
                        if (seen.Add(str))
                            queue.Enqueue(next);
                    }
                }

                level++;
            }

            Console.WriteLine(min);
            result += min;
        }

        return result.ToString();
    }
}