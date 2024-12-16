// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AdventOfCode;

var day = new Day15();
var startTimePart1 = Stopwatch.GetTimestamp();
Console.WriteLine(day.SolvePartOne());
Console.WriteLine($"Time taken for part 1: {Stopwatch.GetElapsedTime(startTimePart1).TotalMilliseconds} ms");
var startTimePart2 = Stopwatch.GetTimestamp();
Console.WriteLine(day.SolvePartTwo());
Console.WriteLine($"Time taken for part 2: {Stopwatch.GetElapsedTime(startTimePart2).TotalMilliseconds} ms");