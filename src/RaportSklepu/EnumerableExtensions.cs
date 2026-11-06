namespace RaportSklepu;

public static class EnumerableExtensions
{
    // STAGE01
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    // STAGE02
    public static TAccumulate Collect<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate collection,
        Action<TAccumulate, TSource> accumulator)
        where TAccumulate : class
    {
        foreach (var item in source)
        {
            accumulator(collection, item);
        }
        return collection;
    }
}
