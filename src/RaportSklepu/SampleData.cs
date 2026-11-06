namespace RaportSklepu;

internal static class SampleData
{
    internal static IReadOnlyList<Customer> Customers { get; } =
    [
        new(101, "Jan", "Nowak"),
        new(102, "Anna", "Kowal"),
        new(103, "Ewa", "Nowak"),
        new(104, "Ola", "Zielińska"),
        new(105, "Adam", "Kowal")
    ];

    internal static IReadOnlyList<LoyaltyCard> Cards { get; } =
    [
        new(102, 0.10m),
        new(103, 0.25m),
        new(105, 0.15m)
    ];

    internal static IReadOnlyList<Product> Products { get; } =
    [
        new(10, "Notes", 10.00m),
        new(20, "Pióro", 8.25m),
        new(30, "Teczka", 4.50m),
        new(40, "Organizer", 14.00m)
    ];

    internal static IReadOnlyList<Order> Orders { get; } =
    [
        new(1004, 103, new DateOnly(2026, 10, 31)),
        new(1001, 101, new DateOnly(2026, 10, 1)),
        new(1005, 101, new DateOnly(2026, 11, 1)),
        new(1002, 102, new DateOnly(2026, 10, 6)),
        new(1000, 104, new DateOnly(2026, 9, 30)),
        new(1003, 102, new DateOnly(2026, 10, 20)),
        new(1006, 103, new DateOnly(2026, 10, 25))
    ];

    internal static IReadOnlyList<OrderLine> Lines { get; } =
    [
        new(1001, 10, 2),
        new(1001, 20, 1),
        new(1002, 20, 2),
        new(1002, 30, 1),
        new(1003, 10, 1),
        new(1003, 30, 2),
        new(1004, 10, 1),
        new(1005, 30, 10),
        new(1000, 10, 7)
    ];
}
