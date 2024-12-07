// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AdventOfCode;

var day = new Day7();
var stopwatch = Stopwatch.StartNew();
Console.WriteLine(day.SolvePartOne());
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms elapsed for part 1");
stopwatch.Restart();
Console.WriteLine(day.SolvePartTwo());
Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms elapsed for part 2");
