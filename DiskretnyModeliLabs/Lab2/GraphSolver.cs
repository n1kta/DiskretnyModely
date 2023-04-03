using Combinatorics.Collections;

namespace DiskretnyModeliLabs.Lab2
{
    public class GraphSolver : Graph<Node>
    {
        List<EdgeExtention> AdditionalEdges = new List<EdgeExtention>();

        private bool IsEilerGraph()
        {
            foreach (List<Node> node in Nodes.Values)
            {
                if (node.Count % 2 != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void FindPostmanWay()
        {
            if (!IsEilerGraph())
            {
                Console.WriteLine("Graph is not Eiler graph. Started graph rebuild.");
                AddEdges();
            }

            FindWay();
        }

        private void FindWay()
        {
            int way = 0;

            foreach (KeyValuePair<char, List<Node>> node in Nodes)
            {
                way += node.Value.Sum(x => x.Weight);
            }

            way /= 2;
            Console.WriteLine($"Path without repeatings: {way}");

            way += AdditionalEdges.Sum(x => x.Length);
            Console.WriteLine($"Full path: {way}");
        }

        public void AddEdges()
        {
            List<char> oddNodes = Nodes.GetOddNodes();

            if (oddNodes.Count % 2 != 0)
            {
                throw new Exception("Wrong input matrix");
            }

            Console.WriteLine("Odd nodes:");
            oddNodes.ForEach(x => Console.Write("{0} ", x));
            Console.WriteLine();

            Dictionary<char, Dictionary<char, EdgeExtention>> matrix = oddNodes
                .Select(x => new
                {
                    Key = x,
                    Dict = Nodes.FindMinimalDistance(x)
                        .Where(res => oddNodes.Contains(res.Key))
                        .ToDictionary(i => i.Key, i => i.Value)
                })
                .ToDictionary(i => i.Key, i => i.Dict);

            var superSet = matrix.SelectMany(x => x.Value.Values).Where(x => x.Length > 0).Distinct().ToList();
            var pairs = new Dictionary<List<string>, int>();
            var filteredSet = new List<EdgeExtention>();

            foreach (EdgeExtention edge in superSet)
            {
                if (!filteredSet.Any(i => i.IsSame(edge.Path)))
                {
                    filteredSet.Add(edge);
                }
            }

            int pairCount = oddNodes.Count / 2;

            AdditionalEdges = new Combinations<EdgeExtention>(filteredSet, pairCount)
                .Where(x => x.CheckIfRepeats())
                .OrderBy(x => x.GetSum())
                .FirstOrDefault()
                .ToList();

            Console.WriteLine("Best additional edges:");
            AdditionalEdges.ForEach(x => Console.WriteLine("Path: {0} Length: {1}", x.Path, x.Length));
        }

        public override void AfterMatrixFill() { }
    }
}
