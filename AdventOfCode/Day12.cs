using System.Numerics;

namespace AdventOfCode;

public sealed class Day12 : IDay
{
    public string SolvePartOne()
    {
        var disjointSetUnion = new DisjointSetUnion();
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

        var perimeter = disjointSetUnion.Sets
            .ToDictionary(region => region, region => Perimeter(region, disjointSetUnion));

        return disjointSetUnion.Sets.Sum(z => disjointSetUnion.Area(z) * perimeter[z]).ToString();
    }

    public string SolvePartTwo()
    {
        var disjointSetUnion = new DisjointSetUnion();
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

        var sides = disjointSetUnion.Sets.ToDictionary(region => region, region => LineSweep(region, disjointSetUnion));

        return disjointSetUnion.Sets.Sum(z => disjointSetUnion.Area(z) * sides[z]).ToString();
    }

    private int Perimeter(Complex region, DisjointSetUnion disjointSetUnion)
    {
        var perimeter = 0;
        for (var i = 0; i < _farm.Length; i++)
        {
            for (var j = 0; j < _farm[0].Length; j++)
            {
                if (disjointSetUnion.Find(i + Complex.ImaginaryOne * j) == region)
                {
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
        }

        return perimeter;
    }

    private int LineSweep(Complex region, DisjointSetUnion disjointSetUnion)
    {
        var topSides = 0;
        var bottomSides = 0;
        var leftSides = 0;
        var rightSides = 0;

        for (var i = 0; i < _farm.Length; i++)
        {
            var topDsu = new DisjointSetUnion();
            var bottomDsu = new DisjointSetUnion();
            var hasNoTopSide = true;
            var hasNoBottomSide = true;

            var tops = new List<Complex>();
            var bottoms = new List<Complex>();

            for (var j = 0; j < _farm[0].Length; j++)
            {
                var current = i + Complex.ImaginaryOne * j;

                if (disjointSetUnion.Find(current) != region)
                {
                    continue;
                }

                if (OutOfBounds(i - 1 + Complex.ImaginaryOne * j) ||
                    disjointSetUnion.Find(i - 1 + Complex.ImaginaryOne * j) != region)
                {
                    tops.Add(current);
                    topDsu.MakeSet(current);
                    hasNoTopSide = false;
                }

                if (OutOfBounds(i + 1 + Complex.ImaginaryOne * j) ||
                    disjointSetUnion.Find(i + 1 + Complex.ImaginaryOne * j) != region)
                {
                    bottoms.Add(current);
                    bottomDsu.MakeSet(current);
                    hasNoBottomSide = false;
                }
            }

            for (var k = 0; k < tops.Count - 1; k++)
            {
                if (tops[k] + Complex.ImaginaryOne == tops[k + 1])
                {
                    topDsu.Union(tops[k], tops[k + 1]);
                }
            }

            for (var k = 0; k < bottoms.Count - 1; k++)
            {
                if (bottoms[k] + Complex.ImaginaryOne == bottoms[k + 1])
                {
                    bottomDsu.Union(bottoms[k], bottoms[k + 1]);
                }
            }
            topSides += hasNoTopSide ? 0 : topDsu.Sets.Count;
            bottomSides += hasNoBottomSide ? 0 : bottomDsu.Sets.Count;
        }

        for (var j = 0; j < _farm[0].Length; j++)
        {
            var leftDsu = new DisjointSetUnion();
            var rightDsu = new DisjointSetUnion();
            var lefts = new List<Complex>();
            var rights = new List<Complex>();
            var hasNoLeftSide = true;
            var hasNoRightSide = true;

            for (var i = 0; i < _farm.Length; i++)
            {
                var current = i + Complex.ImaginaryOne * j;

                if (disjointSetUnion.Find(current) != region)
                {
                    continue;
                }

                if (OutOfBounds(i + Complex.ImaginaryOne * (j - 1)) ||
                    disjointSetUnion.Find(i + Complex.ImaginaryOne * (j - 1)) != region)
                {
                    leftDsu.MakeSet(current);
                    lefts.Add(current);
                    hasNoLeftSide = false;
                }

                if (OutOfBounds(i + Complex.ImaginaryOne * (j + 1)) ||
                    disjointSetUnion.Find(i + Complex.ImaginaryOne * (j + 1)) != region)
                {
                    rightDsu.MakeSet(current);
                    rights.Add(current);
                    hasNoRightSide = false;
                }
            }
            for (var k = 0; k < lefts.Count - 1; k++)
            {
                if (lefts[k] + Complex.One == lefts[k + 1])
                {
                    leftDsu.Union(lefts[k], lefts[k + 1]);
                }
            }

            for (var k = 0; k < rights.Count - 1; k++)
            {
                if (rights[k] + Complex.One == rights[k + 1])
                {
                    rightDsu.Union(rights[k], rights[k + 1]);
                }
            }
            leftSides += hasNoLeftSide ? 0 : leftDsu.Sets.Count;
            rightSides += hasNoRightSide ? 0 : rightDsu.Sets.Count;
        }

        return topSides + bottomSides + leftSides + rightSides;
    }

    private bool OutOfBounds(Complex z) => z.Real < 0 || z.Real >= _farm.Length || z.Imaginary < 0 || z.Imaginary >= _farm[0].Length;

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
            if (!_parent.ContainsKey(z))
            {
                return z;
            }

            return z == _parent[z] ? z : _parent[z] = Find(_parent[z]);
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
}