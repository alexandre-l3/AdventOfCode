namespace AdventOfCode;

public sealed class Day7 : IDay
{
    public string SolvePartOne() => CalculateTotalCalibration(['+', '*']).ToString();

    public string SolvePartTwo() => CalculateTotalCalibration(['+', '*', '|']).ToString();

    private long CalculateTotalCalibration(char[] operations) => _calibrationEquations.Select(e =>
        {
            var equation = e.Split(':');
            return new
            {
                TestValue = long.Parse(equation[0]),
                Terms = equation[1].Trim().Split(' ').Select(long.Parse).ToArray()
            };
        })
        .Where(x => IsCalibrated(operations, x.TestValue, x.Terms, 1, x.Terms.First()))
        .Sum(x => x.TestValue);

    private static bool IsCalibrated(char[] operations, long target, long[] rightTerms, int index, long calculation)
    {
        if (target == calculation && index == rightTerms.Length)
        {
            return true;
        }

        if (index >= rightTerms.Length)
        {
            return false;
        }

        return operations.Select(operation => operation switch
            {
                '+' => calculation + rightTerms[index],
                '*' => calculation * rightTerms[index],
                '|' => long.Parse($"{calculation}{rightTerms[index]}"),
                _ => throw new ArgumentOutOfRangeException()
            })
            .Any(newCalculation => IsCalibrated(operations, target, rightTerms, index + 1, newCalculation));
    }

    private readonly string[] _calibrationEquations =
    [
        "190: 10 19",
        "3267: 81 40 27",
        "83: 17 5",
        "156: 15 6",
        "7290: 6 8 6 15",
        "161011: 16 10 13",
        "192: 17 8 14",
        "21037: 9 7 18 13",
        "292: 11 6 16 20"
    ];
}