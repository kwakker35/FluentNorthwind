using (var client = new FluentNorthwindClient())
{
    // Get all orders
    var orders = await client.Orders().ExecuteAsync();

    // Get a specific order by ID
    var order = await client.Orders().WithId(10248).ExecuteAsync();

    // Get order with related entities expanded
    var orderWithDetails = await client
        .Orders()
        .WithId(10248)
        .Expand("OrderDetails,Customer")
        .ExecuteAsync();

    // Get products with no stock
    var outOfStockProducts = await client.Products().Filter("units_in_stock eq 0").ExecuteAsync();

    // Get employees ordered by first name
    var employeesInOrder = await client.Employees().OrderBy("first_name").ExecuteAsync();

    // Get employees ordered by company name in descending order
    var employeesInDescendingOrder = await client
        .Employees()
        .OrderByDesc("CompanyName")
        .ExecuteAsync();

    // Get top 10 employees ordered by last name
    var top10EmployeesInSurnameOrder = await client
        .Employees()
        .OrderBy("last_name")
        .Top(10)
        .ExecuteAsync();
}
