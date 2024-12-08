using System.Numerics;

namespace AdventOfCode;

public sealed class Day8 : IDay
{
    public string SolvePartOne()
    {
        var antennaMap = MapAntennas();
        var antinodes = new HashSet<Complex>();
        foreach (var antennas in antennaMap.Values)
        {
            foreach (var positionA in antennas)
            {
                foreach (var positionB in antennas.Where(z => z != positionA))
                {
                    var z = positionB - positionA;
                    if (InBounds(positionA - z))
                    {
                        antinodes.Add(positionA - z);
                    }
                    if (InBounds(positionB + z))
                    {
                        antinodes.Add(positionB + z);
                    }
                }
            }
        }
        
        return antinodes.Count.ToString();
    }

    public string SolvePartTwo()
    {
        var antennaMap = MapAntennas();
        var antinodes = new HashSet<Complex>();
        foreach (var antennas in antennaMap.Values)
        {
            foreach (var positionA in antennas)
            {
                foreach (var positionB in antennas.Where(z => z != positionA))
                {
                    var z = positionB - positionA;
                    var current = z;
                    while (InBounds(positionA - current))
                    {
                        antinodes.Add(positionA - current);
                        current += z;
                    }
                    while (InBounds(positionB + current))
                    {
                        antinodes.Add(positionB + current);
                        current += z;
                    }
                }
            }
        }

        return antennaMap.SelectMany(a => a.Value).Union(antinodes).Count().ToString();
    }

    private Dictionary<char, HashSet<Complex>> MapAntennas()
    {
        var antennaMap = new Dictionary<char, HashSet<Complex>>();
        for (var i = 0; i < _map.Length; i++)
        {
            for (var j = 0; j < _map[0].Length; j++)
            {
                if (!char.IsLetterOrDigit(_map[i][j]))
                {
                    continue;
                }

                if (antennaMap.TryGetValue(_map[i][j], out var value))
                {
                    value.Add(new Complex(i, j));
                }
                else
                {
                    antennaMap.Add(_map[i][j], [new Complex(i, j)]);
                }
            }
        }

        return antennaMap;
    }

    private bool InBounds(Complex z) => z.Real >= 0 && z.Real < _map.Length &&
                                        z.Imaginary >= 0 && z.Imaginary < _map[0].Length;
    
    private readonly char[][] _map = _mapInput.Select(x => x.ToCharArray()).ToArray();

    private static readonly string[] _mapInput =
    [
        "............",
        "........0...",
        ".....0......",
        ".......0....",
        "....0.......",
        "......A.....",
        "............",
        "............",
        "........A...",
        ".........A..",
        "............",
        "............"
    ];
}