namespace AdventOfCode._2025;

public sealed class Day6 : IDay
{
    public string SolvePartOne()
    {
        var lines = File.ReadAllLines("2025/input1.txt");
        var numbers = lines.SkipLast(1)
            .Select(x => x.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(long.Parse).ToArray())
            .ToArray();
        var operations = lines.Last().Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        var result = 0L;
        for (var i = 0; i < numbers[0].Length; i++)
        {
            var range = Enumerable.Range(0, numbers.Length);
            var answer = operations[i] switch
            {
                "*" => range.Aggregate(1L, (acc, y) => acc * numbers[y][i]),
                "+" => range.Sum(x => numbers[x][i]),
                _ => throw new InvalidOperationException()
            };
            result += answer;
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var lines = File.ReadAllLines("2025/input1.txt");

        var rows = lines.SkipLast(1).ToArray();
        var operationLine = lines.Last();
        var currentColumn = 0;
        var result = 0L;
        for (var i = 0; i < operationLine.Length; i++)
        {
            if (operationLine[i] != '+' && operationLine[i] != '*')
            {
                continue;
            }
            var j = i + 1;
            var isNotLast = false;
            while (j < operationLine.Length)
            {
                if (operationLine[j] != ' ')
                {
                    isNotLast = true;
                    break;
                }
                j++;
            }

            var size = isNotLast ? j - i - 1 : j - i;

            var answer = operationLine[i] == '+' ? 0L : 1L;
            for (var l = currentColumn; l < currentColumn + size; l++)
            {
                var digits = rows.Aggregate("", (current, t) => current + t[l]);
                var number = long.Parse(digits.Trim());
                answer = operationLine[i] switch
                {
                    '+' => answer + number,
                    '*' => answer * number,
                    _ => throw new InvalidOperationException()
                };
            }

            currentColumn = j;
            result += answer;
        }

        return result.ToString();
    }
}