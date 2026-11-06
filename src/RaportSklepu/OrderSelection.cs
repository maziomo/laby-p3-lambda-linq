using System.Runtime.CompilerServices;

namespace RaportSklepu;

public static class OrderSelection
{
    // STAGE01
    public static Predicate<Order> CreatePeriodFilter(DateOnly fromInclusive, DateOnly toExclusive)
    {
        if (fromInclusive > toExclusive)
        {
            throw new ArgumentOutOfRangeException(nameof(toExclusive));
        }

        Predicate<Order> wynik = bool (Order order) =>
        {
            if (order.PlacedOn >= fromInclusive && order.PlacedOn < toExclusive)
            {
                return true;
            }

            return false;
        };
        return wynik;
    }

    // STAGE02
    public static IReadOnlyDictionary<int, int> CountOrdersByCustomer(IEnumerable<Order> orders)
    {
        Action<Dictionary<int, int>, Order> aktualizacja = (Dictionary<int, int> slownik, Order zamowienie) =>
        {
            if (slownik.ContainsKey(zamowienie.CustomerId)==false)
            {
                slownik.Add(zamowienie.CustomerId, 0);
            }
            slownik[zamowienie.CustomerId]++;
        };
        Dictionary<int, int> wynik = new Dictionary<int, int>();
        return orders.Collect(wynik, aktualizacja);        
    }
}
