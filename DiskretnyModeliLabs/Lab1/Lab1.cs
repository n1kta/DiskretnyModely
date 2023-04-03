namespace DiskretnyModeliLabs.Lab1
{
    public class Lab1
    {
        public static void Execute()
        {
            var array = FileHelper.ReadFromFile("../../../Lab1/Data/l1_3.txt", true);
            var headers = StringHelper.GetNames(array.Length);

            var nodes = CreateGraph(array, headers);
            Invoke(headers, nodes);
        }

        private static void Invoke(char[] headers, List<List<Edge>> nodes)
        {
            var count = 0;
            while (count < nodes.Count - 1)
            {
                var usedNodes = nodes.GetSelected();

                var allNodes = nodes
                    .SelectMany(x => x.Select(i => i))
                    .Where(x => x.Searchable
                        && x.Weight != 0
                        && !x.Selected
                        && x.IsNeeded(usedNodes))
                    .OrderBy(x => x.Weight);
                
                allNodes.First()
                    .Selected = true;

                count = nodes
                    .GetSelected()
                    .Count();

                var stepNodes = nodes.GetSelected();


                stepNodes.Display();

                Console.WriteLine($"Weight: {stepNodes.Select(x => x.Weight).Sum()}");
                Console.WriteLine();
            }
        }

        private static List<List<Edge>> CreateGraph(int[][] array, char[] headers)
        {
            var nodes = new List<List<Edge>>();

            for (int i = 0; i < array.Length; i++)
            {
                nodes.Add(new List<Edge>());

                for (int j = 0; j < array[i].Length; j++)
                    nodes[i].Add(new Edge(array[i][j], $"{headers[i]}{headers[j]}", j > i));
            }

            nodes.Display(headers);

            Console.WriteLine();
            return nodes;
        }
    }
}
