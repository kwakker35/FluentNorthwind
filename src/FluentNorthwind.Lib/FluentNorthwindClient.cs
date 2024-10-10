public class FluentNorthwindClient : IDisposable
{
    private readonly string _baseUrl = "https://services.odata.org/v4/northwind/northwind.svc/";
    private readonly HttpClient _httpClient;

    public FluentNorthwindClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
    }

    public EntityQuery<Order> Orders() => new EntityQuery<Order>(_httpClient, "Orders");

    public EntityQuery<Employee> Employees() => new EntityQuery<Employee>(_httpClient, "Employees");

    public EntityQuery<Product> Products() => new EntityQuery<Product>(_httpClient, "Products");

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
