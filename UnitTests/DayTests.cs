using AdventOfCode;
using FluentAssertions;

namespace UnitTests;

public sealed class DayTests
{
    public static TheoryData<IDay, string> PartOne = new()
    {
        { new Day1(), "11" },
        { new Day2(), "2" },
        { new Day3(), "161" },
        { new Day4(), "18" },
        { new Day5(), "143" },
        { new Day6(), "41" },
        { new Day7(), "3749" },
        { new Day8(), "14" },
        { new Day9(), "1928" },
        { new Day10(), "36" }
    };

    public static TheoryData<IDay, string> PartTwo = new()
    {
        { new Day1(), "31" },
        { new Day2(), "4" },
        { new Day3(), "48" },
        { new Day4(), "9" },
        { new Day5(), "123" },
        { new Day6(), "6" },
        { new Day7(), "11387" },
        { new Day8(), "34" },
        { new Day9(), "2858" },
        { new Day10(), "81" }
    };
    
    [MemberData(nameof(PartOne))]
    [Theory(DisplayName = "Solve advent of code part one ")]
    public void Test1(IDay day, string expected) => day.SolvePartOne().Should().Be(expected);
    
    [MemberData(nameof(PartTwo))]
    [Theory(DisplayName = "Solve advent of code part two ")]
    public void Test2(IDay day, string expected) => day.SolvePartTwo().Should().Be(expected);
}