using DiskretnyModeliLabs.Lab2;

namespace DiskretnyModeliLabs.Lab3
{
    public class SalesmanSolver : Graph<Node>
    {
        private List<Path> Branches = new List<Path>();
        private bool RandomPath = false;
        private Path BasePath => Branches.Where(x => x.IsBase).FirstOrDefault();

        public void FindSalesmanPath(char from)
        {
            if (!Nodes.ContainsKey(from))
            {
                throw new Exception("Node is not found");
            }

            var bestBranch = GetBestPath(from);

            while (bestBranch == null)
            {
                bestBranch = GetBestPath(from);
            }

            Console.Write($"Best path: {string.Join("-", bestBranch.UsedNodes)}");
            Console.WriteLine();
            Console.WriteLine($"Length of the path: ({bestBranch.GetLength(Nodes)})");
        }

        public Path GetBestPath(char from)
        {
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                if (Branches.Count() == 0)
                {
                    Branches = GetBranches(from.ToString(), true);
                }
                else
                {
                    Branches = Branches.SelectMany(x => GetBranches(string.Join("", x.UsedNodes), x.IsBase)).ToList();
                }

                FilterBracnes();

                if (Branches.Count == 0)
                {
                    Console.Clear();
                    return null;
                }
            }

            Branches = Branches.Where(x => Nodes[x.Last].Any(x => x.Name == from)).ToList();

            if (Branches.Count == 0)
            {
                RandomPath = true;
                Console.Clear();
                return null;
            }

            Branches = Branches.Select(x => new Path(false, string.Join("", x.UsedNodes) + from)).ToList();

            return Branches.OrderBy(x => x.CalculateWeight(Nodes)).FirstOrDefault();
        }

        private void FilterBracnes()
        {
            if (Branches.Count != 0)
            {
                if (BasePath == null)
                {
                    Branches.Random().IsBase = true;
                }

                int basePathLength = BasePath.CalculateWeight(Nodes);

                Branches = Branches.Where(x => x.IsBase || x.ComparePath(basePathLength, Nodes)).ToList();
            }
            else
            {
                RandomPath = true;
            }
        }

        private List<Path> GetBranches(string path, bool hasBase)
        {
            List<Node> nodes = Nodes[path.Last()].Where(x => !path.Contains(x.Name)).ToList();
            int i = 0;
            if (RandomPath)
            {
                Random rnd = new Random();
                i = rnd.Next(0, nodes.Count);
            }

            List<Path> branches = nodes.Select((x, index) => index == i
                ? new Path(isBase: hasBase, node: path + x.Name.ToString())
                : new Path(node: path + x.Name.ToString())).ToList();

            return branches;
        }

        public override void AfterMatrixFill() { }
    }

    public class Path
    {
        public bool IsBase { get; set; } = false;
        public List<char> UsedNodes { get; private set; } = new List<char>();
        public char Last => UsedNodes.Last();

        public Path(string node)
        {
            UsedNodes = node.ToList();
        }

        public Path(bool isBase, string node) : this(node)
        {
            IsBase = isBase;
        }

        public int GetLength(Dictionary<char, List<Node>> nodes)
        {
            int weight = 0;
            for (int i = 0; i < UsedNodes.Count - 1; i++)
            {
                weight += nodes[UsedNodes[i]].FirstOrDefault(x => x.Name == UsedNodes[i + 1]).Weight;
            }

            return weight;
        }

        public int CalculateWeight(Dictionary<char, List<Node>> nodes)
        {
            var weight = 0;
            var notUsed = nodes;

            for (int i = 0; i < UsedNodes.Count - 1; i++)
            {
                weight += nodes[UsedNodes[i]].FirstOrDefault(x => x.Name == UsedNodes[i + 1]).Weight;
                notUsed = notUsed
                    .Where(x => x.Key != UsedNodes[i])
                    .ToDictionary(i => i.Key, j => j.Value.Where(x => x.Name != UsedNodes[i + 1]).ToList());
            }

            var minValues = notUsed
                .ToDictionary(i => i.Key, x => x.Value.Count > 0 ? x.Value.Min(i => i.Weight) : 0);

            weight += minValues.Sum(x => x.Value);

            var transformed = notUsed
                .ToDictionary(i => i.Key, j => j.Value.Select(x => new Node
                {
                    Name = x.Name,
                    Weight = x.Weight - minValues[j.Key]
                })
                .ToList());

            var thirdStep = 0;
            var nodesForTransformed = nodes.Keys.Where(x => !UsedNodes.Contains(x));

            foreach (char node in nodesForTransformed)
            {
                var column = transformed.SelectMany(x => x.Value.Where(i => i.Name == node));
                thirdStep += column.Count() > 0
                    ? column.Min(x => x.Weight)
                    : 0;
            }

            weight += thirdStep;

            return weight;
        }

        public bool ComparePath(int basePathLength, Dictionary<char, List<Node>> nodes)
        {
            int pathLength = CalculateWeight(nodes);
            return pathLength <= basePathLength;
        }
    }
}
