using System.Numerics;

namespace AdventOfCode;

public sealed class Day12 : IDay
{
    public string SolvePartOne()
    {
        var disjointSetUnion = new DisjointSetUnion();
        TraverseFarm(disjointSetUnion);

        var perimeter = disjointSetUnion.Sets
            .ToDictionary(region => region, region => Perimeter(region, disjointSetUnion));

        return disjointSetUnion.Sets.Sum(z => disjointSetUnion.Area(z) * perimeter[z]).ToString();
    }

    public string SolvePartTwo()
    {
        var disjointSetUnion = new DisjointSetUnion();
        TraverseFarm(disjointSetUnion);

        var sides = disjointSetUnion.Sets.ToDictionary(region => region, region => LineSweep(region, disjointSetUnion));

        return disjointSetUnion.Sets.Sum(z => disjointSetUnion.Area(z) * sides[z]).ToString();
    }

    private void TraverseFarm(DisjointSetUnion disjointSetUnion)
    {
        for (var i = 0; i < _farm.Length; i++)
        {
            for (var j = 0; j < _farm[0].Length; j++)
            {
                disjointSetUnion.MakeSet(i + Complex.ImaginaryOne * j);
            }
        }

        var queue = new Queue<Complex>([Complex.Zero]);
        var visited = new HashSet<Complex>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            foreach (var direction in _directions)
            {
                var next = node + direction;
                if (OutOfBounds(next))
                {
                    continue;
                }
                if (_farm[(int)next.Real][(int)next.Imaginary] == _farm[(int)node.Real][(int)node.Imaginary])
                {
                    disjointSetUnion.Union(node, next);
                }
                if (visited.Add(next))
                {
                    queue.Enqueue(next);
                }
            }
        }
    }

    private int Perimeter(Complex region, DisjointSetUnion disjointSetUnion)
    {
        var perimeter = 0;
        for (var i = 0; i < _farm.Length; i++)
        {
            for (var j = 0; j < _farm[0].Length; j++)
            {
                if (disjointSetUnion.Find(i + Complex.ImaginaryOne * j) != region)
                {
                    continue;
                }
                perimeter += 4;
                if (i > 0 && disjointSetUnion.Find(i - 1 + Complex.ImaginaryOne * j) == region)
                {
                    perimeter -= 2;
                }

                if (j > 0 && disjointSetUnion.Find(i + Complex.ImaginaryOne * (j - 1)) == region)
                {
                    perimeter -= 2;
                }
            }
        }

        return perimeter;
    }

    private int LineSweep(Complex region, DisjointSetUnion disjointSetUnion) =>
        VerticalSweep(region, disjointSetUnion) + HorizontalSweep(region, disjointSetUnion);

    private int HorizontalSweep(Complex region, DisjointSetUnion disjointSetUnion)
    {
        var leftSides = 0;
        var rightSides = 0;
        for (var j = 0; j < _farm[0].Length; j++)
        {
            var leftDsu = new DisjointSetUnion();
            var rightDsu = new DisjointSetUnion();
            var lefts = new List<Complex>();
            var rights = new List<Complex>();

            for (var i = 0; i < _farm.Length; i++)
            {
                var current = i + Complex.ImaginaryOne * j;

                if (disjointSetUnion.Find(current) != region)
                {
                    continue;
                }

                if (OutOfBounds(current - Complex.ImaginaryOne) ||
                    disjointSetUnion.Find(current - Complex.ImaginaryOne) != region)
                {
                    leftDsu.MakeSet(current);
                    lefts.Add(current);
                }

                if (OutOfBounds(current + Complex.ImaginaryOne) ||
                    disjointSetUnion.Find(current + Complex.ImaginaryOne) != region)
                {
                    rightDsu.MakeSet(current);
                    rights.Add(current);
                }
            }
            ConnectLine(leftDsu, lefts, Complex.One);
            ConnectLine(rightDsu, rights, Complex.One);
            leftSides += leftDsu.Sets.Count;
            rightSides += rightDsu.Sets.Count;
        }

        return leftSides + rightSides;
    }

    private int VerticalSweep(Complex region, DisjointSetUnion disjointSetUnion)
    {
        var topSides = 0;
        var bottomSides = 0;
        for (var i = 0; i < _farm.Length; i++)
        {
            var topDsu = new DisjointSetUnion();
            var bottomDsu = new DisjointSetUnion();
            var tops = new List<Complex>();
            var bottoms = new List<Complex>();

            for (var j = 0; j < _farm[0].Length; j++)
            {
                var current = i + Complex.ImaginaryOne * j;

                if (disjointSetUnion.Find(current) != region)
                {
                    continue;
                }

                if (OutOfBounds(current - Complex.One) ||
                    disjointSetUnion.Find(current - Complex.One) != region)
                {
                    tops.Add(current);
                    topDsu.MakeSet(current);
                }

                if (OutOfBounds(current + Complex.One) ||
                    disjointSetUnion.Find(current + Complex.One) != region)
                {
                    bottoms.Add(current);
                    bottomDsu.MakeSet(current);
                }
            }
            ConnectLine(topDsu, tops, Complex.ImaginaryOne);
            ConnectLine(bottomDsu, bottoms, Complex.ImaginaryOne);
            topSides += topDsu.Sets.Count;
            bottomSides += bottomDsu.Sets.Count;
        }

        return topSides + bottomSides;
    }

    private static void ConnectLine(DisjointSetUnion disjointSetUnion, List<Complex> line, Complex direction)
    {
        for (var k = 0; k < line.Count - 1; k++)
        {
            if (line[k] + direction == line[k + 1])
            {
                disjointSetUnion.Union(line[k], line[k + 1]);
            }
        }
    }

    private bool OutOfBounds(Complex z) => z.Real < 0 || z.Real >= _farm.Length ||
                                           z.Imaginary < 0 || z.Imaginary >= _farm[0].Length;

    private readonly Complex[] _directions = [Complex.One, -Complex.One, Complex.ImaginaryOne, -Complex.ImaginaryOne];

    private sealed class DisjointSetUnion
    {
        private readonly Dictionary<Complex, Complex> _parent = new();
        private readonly Dictionary<Complex, int> _size = new();

        public IReadOnlyList<Complex> Sets => _parent.Values.Distinct().ToList();

        public void MakeSet(Complex z)
        {
            _parent[z] = z;
            _size[z] = 1;
        }

        public int Area(Complex z) => _parent.Count(item => z == Find(item.Key));

        public Complex Find(Complex z)
        {
            if (!_parent.TryGetValue(z, out var value))
            {
                return z;
            }

            return z == value ? z : _parent[z] = Find(value);
        }

        public void Union(Complex a, Complex b)
        {
            var rootA = Find(a);
            var rootB = Find(b);
            if (rootA == rootB)
            {
                return;
            }

            if (_size[rootA] < _size[rootB])
            {
                (rootA, rootB) = (rootB, rootA);
            }

            _parent[rootB] = rootA;
            _size[rootA] += _size[rootB];
        }
    }

    private readonly string[] _farm =
    [
        "RRRRIICCFF",
        "RRRRIICCCF",
        "VVRRRCCFFF",
        "VVRCCCJFFF",
        "VVVVCJJCFE",
        "VVIVCCJJEE",
        "VVIIICJJEE",
        "MIIIIIJJEE",
        "MIIISIJEEE",
        "MMMISSJEEE"
    ];
}