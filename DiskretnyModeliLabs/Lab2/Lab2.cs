namespace DiskretnyModeliLabs.Lab2
{
    public class Lab2
    {
        public static void Execute()
        {
            int[][] array = FileHelper.ReadFromFile("../../../Lab2/Data/l2-1.txt", true);
            char[] headers = StringHelper.GetNames(array.Length);

            var graph = new GraphSolver();
            graph.FillFromMatrix(headers, array);

            graph.FindPostmanWay();
        }
    }
}
