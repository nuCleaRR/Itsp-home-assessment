namespace Api.IntegrationTests.Helpers;

public static class EnumerableExtension
{
    private static readonly Random _rnd = new();
    
    public static T PickRandom<T>(this List<T> source)
    {
        var index = _rnd.Next(source.Count);

        return source[index];
    }
}