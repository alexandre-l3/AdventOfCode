using System.Text.RegularExpressions;

namespace AdventOfCode;

public sealed class Day3 : IDay
{
    public string SolvePartOne()
    {
        var regex = new Regex(@"mul\(\d+,\d+\)");
        var match = regex.Match(Memory1);
        var result = 0;
        while (match.Success)
        {
            var group = match.Groups[0].Value.Replace("mul(", "").Replace(")", "").Split(",");
            result += int.Parse(group[0]) * int.Parse(group[1]);
            match = match.NextMatch();
        }

        return result.ToString();
    }

    public string SolvePartTwo()
    {
        var regex = new Regex(@"(mul\(\d+,\d+\)|(do\(\)|don\'t\(\)))");
        var match = regex.Match(Memory2);
        var result = 0;
        var doing = true;
        while (match.Success)
        {
            var group = match.Groups[0].Value;
            if (doing)
            {
                if (group.StartsWith("mul"))
                {
                    var values = group.Replace("mul(", "").Replace(")", "").Split(",");
                    result += int.Parse(values[0]) * int.Parse(values[1]);
                }
                else if (group.StartsWith("don't"))
                {
                    doing = false;
                }
            }
            else
            {
                if (group.StartsWith("do()"))
                {
                    doing = true;
                }
            }

            match = match.NextMatch();
        }

        return result.ToString();
    }

    private const string Memory1 = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
    private const string Memory2 = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
}