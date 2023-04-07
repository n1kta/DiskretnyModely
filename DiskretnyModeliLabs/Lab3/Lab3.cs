namespace DiskretnyModeliLabs.Lab3
{
    public class Lab3
    {
        public static void Execute()
        {
            int[][] array = FileHelper.ReadFromFile("../../../Lab3/Data/l3-1.txt", true);
            char[] headers = StringHelper.GetNames(array.Length);

            var graph = new SalesmanSolver();
            graph.FillFromMatrix(headers, array);

            graph.FindSalesmanPath('F');
        }
    }
}
