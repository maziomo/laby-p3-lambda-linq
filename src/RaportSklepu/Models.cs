namespace RaportSklepu;

public sealed record Customer(int Id, string FirstName, string LastName);

public sealed record LoyaltyCard(int CustomerId, decimal DiscountRate);

public sealed record Product(int Id, string Name, decimal UnitPrice);

public sealed record Order(int Id, int CustomerId, DateOnly PlacedOn);

public sealed record OrderLine(int OrderId, int ProductId, int Quantity);

public sealed record ProductSales(int ProductId, string Name, long UnitsSold);

public sealed record CustomerSpending(int CustomerId, string FirstName, string LastName, decimal TotalPaid);
