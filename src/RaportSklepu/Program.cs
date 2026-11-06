using RaportSklepu;

var from = new DateOnly(2026, 10, 1);
var to = new DateOnly(2026, 11, 1);

Console.WriteLine("Raport sklepu papierniczego za październik 2026");

RunStage("STAGE01", "Zamówienia w okresie", () =>
{
    Console.WriteLine(string.Join(", ", OrdersInPeriod().Select(order => order.Id)));
});

RunStage("STAGE02", "Liczba zamówień według klienta", () =>
{
    var counts = OrderSelection.CountOrdersByCustomer(OrdersInPeriod());
    foreach (var (customerId, count) in counts.OrderBy(pair => pair.Key))
    {
        Console.WriteLine($"{customerId} | {count}");
    }
});

RunStage("STAGE03", "Klienci bez karty", () =>
{
    foreach (var customer in ShopReports.CustomersWithoutCard(SampleData.Customers, SampleData.Cards))
    {
        Console.WriteLine($"{customer.Id} | {customer.LastName}, {customer.FirstName}");
    }
});

RunStage("STAGE04", "Najczęściej zamawiane produkty", () =>
{
    var ranking = ShopReports.TopProducts(SampleData.Products, OrdersInPeriod(), SampleData.Lines);
    foreach (var row in ranking)
    {
        Console.WriteLine($"{row.ProductId} | {row.Name} | {row.UnitsSold}");
    }
});

RunStage("STAGE05", "Wydatki po rabacie", () =>
{
    var spending = ShopReports.SpendingByCustomer(
        SampleData.Customers, SampleData.Cards, SampleData.Products, OrdersInPeriod(), SampleData.Lines);
    foreach (var row in spending)
    {
        Console.WriteLine(FormattableString.Invariant(
            $"{row.CustomerId} | {row.LastName}, {row.FirstName} | {row.TotalPaid:F2} PLN"));
    }
});

IEnumerable<Order> OrdersInPeriod()
{
    return SampleData.Orders.Filter(OrderSelection.CreatePeriodFilter(from, to));
}

static void RunStage(string stage, string description, Action run)
{
    Console.WriteLine();
    Console.WriteLine($"{stage} — {description}");
    try
    {
        run();
    }
    catch (NotImplementedException exception)
    {
        Console.WriteLine($"Brak implementacji ({exception.Message}).");
    }
}
