namespace DiskretnyModeliLabs.Lab2;

public class EdgeExtention
{
    public string Path { get; private set; }
    public int Length { get; private set; }

    public EdgeExtention(string path, int length)
    {
        Path = path;
        Length = length;
    }

    public bool Intersect(string other)
    {
        var combination = $"{Path.First()}{Path.Last()}{other.First()}{other.Last()}";
        var orderedCombination = combination.OrderBy(x => x);
        var distinctCombination = combination.Distinct().OrderBy(x => x);

        return string.Join(string.Empty, orderedCombination) == string.Join(string.Empty, distinctCombination);
    }

    public bool IsSame(string other)
        => string.Join(string.Empty, Path.OrderBy(x => x)) == string.Join(string.Empty, other.OrderBy(x => x));
}