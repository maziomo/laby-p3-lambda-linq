using System.Collections.Immutable;
using System.ComponentModel;

namespace RaportSklepu;

public static class ShopReports
{
    // STAGE03
    public static IReadOnlyList<Customer> CustomersWithoutCard(
        IEnumerable<Customer> customers,
        IEnumerable<LoyaltyCard> cards)
    {
        /*
        List<Customer> wynik = new List<Customer>();
        foreach (var customer in customers)
        {
            bool contains = false;
            foreach (var card in cards)
            {
                if (customer.Id == card.CustomerId)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                wynik.Add(customer);
            }
        }
        wynik = wynik.OrderBy(x => x.LastName).ThenBy(x=>x.FirstName).ThenBy(x=>x.Id).ToList();
        return wynik;
        */
        
        var zkarta = customers.Join(cards, customer=>customer.Id, card=>card.CustomerId,
            (customer, card) => customer.Id);
        var bezkarty = customers.Where(customer => !zkarta.Contains(customer.Id));
        bezkarty = bezkarty.OrderBy(customer => customer.LastName, StringComparer.Ordinal)
            .ThenBy(customer => customer.FirstName, StringComparer.Ordinal)
            .ThenBy(customer=>customer.Id).ToList();
        return bezkarty.ToList();
    }

    // STAGE04
    public static IReadOnlyList<ProductSales> TopProducts(
        IEnumerable<Product> products,
        IEnumerable<Order> orders,
        IEnumerable<OrderLine> lines,
        int limit = 5)
    {
        if (limit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(limit));
        }
        var pozycjeZNazwami = products.Join(
            lines,
            product => product.Id,
            line => line.ProductId,
            (product, line) => (product.Name, line.ProductId, line.OrderId, line.Quantity));
        var pozycjeWRaporcie = pozycjeZNazwami.Join(
            orders,
            pozycja => pozycja.OrderId,
            order => order.Id,
            (pozycja, order) => pozycja);
        var sprzedaz = pozycjeWRaporcie
            .GroupBy(pozycja => (pozycja.ProductId, pozycja.Name))
            .Select(grupa => new ProductSales(
                grupa.Key.ProductId,
                grupa.Key.Name,
                grupa.Sum(pozycja => (long)pozycja.Quantity)));
        return sprzedaz
            .OrderByDescending(produkt => produkt.UnitsSold)
            .ThenBy(produkt => produkt.ProductId)
            .Take(limit)
            .ToList();
        
    }

    // STAGE05
    public static IReadOnlyList<CustomerSpending> SpendingByCustomer(
        IEnumerable<Customer> customers,
        IEnumerable<LoyaltyCard> cards,
        IEnumerable<Product> products,
        IEnumerable<Order> orders,
        IEnumerable<OrderLine> lines)
    {
        throw new NotImplementedException("STAGE05: SpendingByCustomer");
    }
}
