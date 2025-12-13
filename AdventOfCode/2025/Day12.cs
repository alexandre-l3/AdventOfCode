namespace AdventOfCode._2025;

public sealed class Day12 : IDay
{
    public string SolvePartOne()
    {
        using var stream = new StreamReader("2025/input1.txt");

        var count = 1;
        var result = 0;
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine()!;
            if (count > 30)
            {
                var query = line.Replace(":", "").Split(' ');
                var region = query[0].Split('x');
                var columns = int.Parse(region[0]);
                var rows = int.Parse(region[1]);

                var capacity = columns * rows;
                var quantity = query.Skip(1).Select(int.Parse).ToArray();

                result += 9 * quantity.Where(x => x > 0).Sum() <= capacity ? 1 : 0;
            }

            count++;
        }

        return result.ToString();
    }

    public string SolvePartTwo() => "No part 2";
}