namespace AdventOfCode._2025;

public sealed class Day1 : IDay
{
    private const int Start = 50;

    public string SolvePartOne()
    {
        using var file = new StreamReader("2025/input1.txt");
        var dial = Start;
        var password = 0;
        while (!file.EndOfStream)
        {
            var line = file.ReadLine();
            var rotation = int.Parse(line[0] == 'L' ? $"-{line[1..]}" : line[1..]);
            dial += rotation;
            dial %= 100;
            if (dial < 0)
            {
                dial += 100;
            }

            if (dial == 0)
            {
                password++;
            }
        }

        return password.ToString();
    }

    public string SolvePartTwo()
    {
        using var file = new StreamReader("2025/input1.txt");
        var dial = Start;
        var password = 0;
        while (!file.EndOfStream)
        {
            var line = file.ReadLine();
            var rotation = int.Parse(line[0] == 'L' ? $"-{line[1..]}" : line[1..]);
            var previousDial = dial;
            dial += rotation;
            var revolutions = Math.Abs(rotation / 100);
            if (dial > 100)
            {
                dial %= 100;
                if (previousDial > dial)
                {
                    revolutions++;
                }

                password += revolutions;
            }

            else if (dial < 0)
            {
                dial %= 100;
                dial += 100;
                if (dial > previousDial && previousDial > 0)
                {
                    revolutions++;
                }

                dial %= 100;
                password += revolutions;
            }

            else if (dial is 0 or 100)
            {
                password++;
                dial = 0;
            }
        }

        return password.ToString();
    }
}