namespace AdventOfCode._2025;

public sealed class Day3 : IDay
{
    public string SolvePartOne()
    {
        using var stream = new StreamReader("2025/input1.txt");
        var totalJoltage = 0;
        while (!stream.EndOfStream)
        {
            var bank = stream.ReadLine()
                .ToCharArray()
                .Select(x => x - '0')
                .ToArray();

            var joltage = int.MinValue;
            for (var i = 0; i < bank.Length - 1; i++)
            {
                for (var j = i + 1; j < bank.Length; j++)
                {
                    joltage = Math.Max(joltage, bank[i] * 10 + bank[j]);
                }
            }

            totalJoltage += joltage;
        }

        return totalJoltage.ToString();
    }

    public string SolvePartTwo()
    {
        using var stream = new StreamReader("2025/input1.txt");
        var totalJoltage = 0L;
        while (!stream.EndOfStream)
        {
            var bank = stream.ReadLine()
                .ToCharArray()
                .Select(x => x - '0')
                .ToArray();
            var joltage = 0L;
            var position = 0;
            for (var i = 0; i < 12; i++)
            {
                var largestDigit = 0;
                for (var j = position; j < bank.Length + i - 11; j++)
                {
                    if (largestDigit >= bank[j])
                    {
                        continue;
                    }
                    position = j;
                    largestDigit = bank[j];
                }

                joltage = joltage * 10 + largestDigit;
                position++;
            }

            totalJoltage += joltage;
        }

        return totalJoltage.ToString();
    }
}