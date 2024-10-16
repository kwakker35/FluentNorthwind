using FluentNorthwind.Lib.Model;

namespace FluentNorthwind.Lib;

public class FluentNorthwindClient : IDisposable
{
    private readonly string _baseUrl = "https://services.odata.org/v4/northwind/northwind.svc/";
    private readonly HttpClient _httpClient;

    public FluentNorthwindClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
    }

    public EntityQuery<Order> Orders() => new EntityQuery<Order>(_httpClient, "Orders");

    public EntityQuery<Order> Orders(int id) =>
        new EntityQuery<Order>(_httpClient, "Orders").WithId(id);

    public EntityQuery<Employee> Employees() => new EntityQuery<Employee>(_httpClient, "Employees");

    public EntityQuery<Employee> Employees(int id) =>
        new EntityQuery<Employee>(_httpClient, "Employees").WithId(id);

    public EntityQuery<Product> Products() => new EntityQuery<Product>(_httpClient, "Products");

    public EntityQuery<Product> Products(int id) =>
        new EntityQuery<Product>(_httpClient, "Products").WithId(id);

    public EntityQuery<Category> Categories() =>
        new EntityQuery<Category>(_httpClient, "Categories");

    public EntityQuery<Category> Categories(int id) =>
        new EntityQuery<Category>(_httpClient, "Categories").WithId(id);

    public EntityQuery<Customer> Customers() => new EntityQuery<Customer>(_httpClient, "Customers");

    public EntityQuery<Customer> Customers(string id) =>
        new EntityQuery<Customer>(_httpClient, "Customers").WithId(id);

    public EntityQuery<Supplier> Suppliers() => new EntityQuery<Supplier>(_httpClient, "Suppliers");

    public EntityQuery<Supplier> Suppliers(int id) =>
        new EntityQuery<Supplier>(_httpClient, "Suppliers").WithId(id);

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
