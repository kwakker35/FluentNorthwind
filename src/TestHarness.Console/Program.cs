using FluentNorthwind.Lib;

using (var client = new FluentNorthwindClient())
{
    // Get all orders
    var orders = await client.Orders().ExecuteAsync();

    // Get a specific order by ID
    var order = await client.Orders(10248).ExecuteAsync();

    // Get order with related entities expanded
    var orderWithDetails = await client
        .Orders(10248)
        .Expand("Order_Details,Customer")
        .ExecuteAsync();

    // Get products with no stock
    var outOfStockProducts = await client.Products().Filter("UnitsInStock eq 0").ExecuteAsync();

    // Get employees ordered by first name
    var employeesInOrder = await client.Employees().OrderBy("FirstName").ExecuteAsync();

    // Get employees ordered by company name in descending order
    var employeesInDescendingOrder = await client.Employees().OrderByDesc("City").ExecuteAsync();

    // Get top 10 employees ordered by last name
    var top10EmployeesInSurnameOrder = await client
        .Employees()
        .OrderBy("LastName")
        .Top(10)
        .ExecuteAsync();

    // Get Employees but only certain fields
    var basicEmployees = await client
        .Employees()
        .Select(
            e =>
                new
                {
                    e.FirstName,
                    e.LastName,
                    e.City
                }
        )
        .ExecuteAsync();

    // Get Employees but only certain fields
    var employeesCity = await client.Employees().Select(e => e.City).ExecuteAsync();

    //get single customer by id
    var customer = await client.Customers("ALFKI").ExecuteAsync();
}
