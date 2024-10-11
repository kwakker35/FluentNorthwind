
# FluentNorthwindClient

**FluentNorthwindClient** is a C# library designed to interact with the Northwind OData service in a fluent and intuitive way. It allows developers to build OData queries seamlessly using a fluent API, abstracting the complexity of URL construction and query string manipulation. The library returns strongly-typed entities and is built with a focus on maintaining SOLID and DRY principles.

## What is a Fluent API?

A **Fluent API** provides an interface that allows method chaining, creating more readable and expressive code. Instead of having to construct complex queries using strings or individual method calls, the developer can use a chain of methods to achieve the desired result. This pattern is prevalent in libraries like LINQ and MS Graph SDK.

The primary benefit is **readability**. Fluent APIs allow you to write code that flows naturally, like reading a sentence, which makes the intent of the code more clear and concise.

## Key Features of FluentNorthwindClient

- **Entity Queries**: Supports querying `Orders`, `OrderDetails`, `Products`, and `Employees` from the Northwind OData service.
- **OData Query Support**: Fluent query building for filters, ordering, expanding related entities, selecting specific fields, and pagination using `Top`.
- **Strongly-Typed Entities**: Results are mapped to strongly-typed C# classes, providing intellisense support and eliminating magic strings.
- **Customizable Queries**: Build flexible and dynamic OData queries on the fly.
  
## How to Use

Here’s a quick guide on how to get started with the **FluentNorthwindClient**.

### Installation

Simply clone the repository and include the project in your solution. Alternatively, you can add the compiled library as a reference to your project.

### Example Usage

The following examples showcase how to interact with the Northwind OData service using the fluent API. Queries are built using method chaining and executed asynchronously.

#### Fetch All Orders

```csharp
var client = new FluentNorthwindClient();
var orders = await client.Orders().ExecuteAsync();

foreach (var order in orders)
{
    Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerId}");
}
```

#### Fetch a Single Order by ID

```csharp
var order = await client.Orders().WithId(10249).ExecuteAsync();
Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerId}");
```

#### Fetch Order with Expanded Related Entities

You can expand related entities such as `OrderDetails` or `Customer`:

```csharp
var orderWithDetails = await client.Orders()
                                   .WithId(10249)
                                   .Expand("OrderDetails,Customer")
                                   .ExecuteAsync();

Console.WriteLine($"Order ID: {orderWithDetails.OrderId}, Customer: {orderWithDetails.Customer.CompanyName}");
```

#### Filter Products That Are Out of Stock

```csharp
var outOfStockProducts = await client.Products()
                                     .Filter("UnitsInStock eq 0")
                                     .ExecuteAsync();

foreach (var product in outOfStockProducts)
{
    Console.WriteLine($"Product: {product.ProductName}, Units in Stock: {product.UnitsInStock}");
}
```

#### Select Specific Fields

You can retrieve only specific fields from an entity. For example, to fetch employees with just their `FirstName`, `LastName`, and `City`:

```csharp
var employees = await client.Employees()
                            .Select("FirstName, LastName, City")
                            .ExecuteAsync();

foreach (var employee in employees)
{
    Console.WriteLine($"{employee.FirstName} {employee.LastName}, City: {employee.City}");
}
```

#### Sorting and Pagination

You can order the results and limit the number of items returned using the `OrderBy`, `OrderByDesc`, and `Top` methods:

```csharp
var top10Employees = await client.Employees()
                                 .OrderBy("LastName")
                                 .Top(10)
                                 .ExecuteAsync();

foreach (var employee in top10Employees)
{
    Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
}
```

## Key Components

- **`FluentNorthwindClient`**: The main entry point for interacting with the Northwind OData service.
- **`EntityQuery<T>`**: A generic class for constructing OData queries for entities like `Orders`, `Products`, `Employees`, etc.
- **OData Query Methods**: Supports `Filter()`, `Expand()`, `OrderBy()`, `OrderByDesc()`, `Select()`, and `Top()`.

## How It Works

FluentNorthwindClient leverages a fluent API pattern to enable chaining of query options like filters and sorting. Each method call adds to the OData query string, which is then executed via HTTP when `ExecuteAsync()` is called. The response is deserialized into strongly-typed C# objects based on the requested entity.

For example, the following call:
```csharp
var employees = await client.Employees().OrderBy("LastName").Top(10).ExecuteAsync();
```

Will build the query:
```
https://services.odata.org/v4/northwind/northwind.svc/Employees?$orderby=LastName&$top=10
```

## Additional Resources

- Learn more about **Fluent Interface Design**: [Fluent Interface on Wikipedia](https://en.wikipedia.org/wiki/Fluent_interface)
- Dive into **Fluent APIs** and how they improve readability: [Fluent Interface Pattern](https://martinfowler.com/bliki/FluentInterface.html)
- Explore **OData** and its query capabilities: [Northwind OData Service](https://services.odata.org/V4/Northwind/Northwind.svc)

## License

This project is open-source under the MIT License.
