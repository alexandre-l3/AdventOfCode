using AdventOfCode;
using FluentAssertions;

namespace UnitTests;

public class DayTests
{
    public static TheoryData<IDay, string> PartOne = new()
    {
        { new Day1(), "11" },
        { new Day2(), "2" }
    };

    public static TheoryData<IDay, string> PartTwo = new()
    {
        { new Day1(), "31" },
        { new Day2(), "4" }
    };
    
    [MemberData(nameof(PartOne))]
    [Theory(DisplayName = "Solve advent of code part one ")]
    public void Test1(IDay day, string expected) => day.SolveFirst().Should().Be(expected);
    
    [MemberData(nameof(PartTwo))]
    [Theory(DisplayName = "Solve advent of code part two ")]
    public void Test2(IDay day, string expected) => day.SolveSecond().Should().Be(expected);
}