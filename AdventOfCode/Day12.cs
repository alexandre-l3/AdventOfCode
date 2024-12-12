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

        foreach (var region in disjointSetUnion.Sets)
        {
            Console.WriteLine($"Region {_farm[(int)region.Real][(int)region.Imaginary]} : {Bfs(region, disjointSetUnion)}");
        }

        return disjointSetUnion.Sets.Sum(z => disjointSetUnion.Area(z) * disjointSetUnion.Perimeter(z)).ToString();
    }

    public string SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private int Bfs(Complex start, DisjointSetUnion disjointSetUnion)
    {
        var queue = new Queue<Complex>([start]);
        var visited = new HashSet<Complex>();

        var perimeter = 0;
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            foreach (var direction in _directions)
            {
                var next = node + direction;
                if (visited.Contains(next))
                {
                    continue;
                }
                if (OutOfBounds(next))
                {
                    perimeter++;
                    continue;
                }

                if (disjointSetUnion.Find(node) != disjointSetUnion.Find(next))
                {
                    perimeter++;
                }

                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        return perimeter;
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
        private readonly Dictionary<Complex, Complex> _parent;
        private readonly Dictionary<Complex, int> _size;

        public DisjointSetUnion()
        {
            _parent = new Dictionary<Complex, Complex>();
            _size = new Dictionary<Complex, int>();
        }

        public IReadOnlyList<Complex> Sets => _parent.Values.Distinct().ToList();

        public void MakeSet(Complex z)
        {
            _parent[z] = z;
            _size[z] = 1;
        }

        public int Area(Complex z) => _parent.Count(item => z == Find(item.Key));

        public int Perimeter(Complex z)
        {
            return 0;
        }

        public Complex Find(Complex z)
        {
            if (!_parent.ContainsKey(z))
            {
                _parent[z] = z;
                _size[z] = 1;
            }

            return z == _parent[z] ? z : (_parent[z] = Find(_parent[z]));
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