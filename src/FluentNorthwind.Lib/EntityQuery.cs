using Newtonsoft.Json;

public class EntityQuery<T>
{
    private readonly HttpClient _httpClient;
    private string _entitySetName;
    private string _filter;
    private string _orderBy;
    private int? _top;
    private string _expand;

    public EntityQuery(HttpClient httpClient, string entitySetName)
    {
        _httpClient = httpClient;
        _entitySetName = entitySetName;
    }

    public EntityQuery<T> Filter(string filter)
    {
        _filter = filter;
        return this;
    }

    public EntityQuery<T> OrderBy(string orderBy)
    {
        _orderBy = orderBy;
        return this;
    }

    public EntityQuery<T> OrderByDesc(string orderBy)
    {
        _orderBy = $"{orderBy} desc";
        return this;
    }

    public EntityQuery<T> Top(int top)
    {
        _top = top;
        return this;
    }

    public EntityQuery<T> Expand(string expand)
    {
        _expand = expand;
        return this;
    }

    public EntityQuery<T> WithId(int id)
    {
        _entitySetName += $"({id})";
        return this;
    }

    public async Task<IEnumerable<T>> ExecuteAsync()
    {
        var query = BuildQuery();
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ODataResponse<T>>(json)?.Value;
    }

    private string BuildQuery()
    {
        var query = $"{_entitySetName}?";

        if (!string.IsNullOrEmpty(_filter))
        {
            query += $"$filter={_filter}&";
        }

        if (!string.IsNullOrEmpty(_orderBy))
        {
            query += $"$orderby={_orderBy}&";
        }

        if (_top.HasValue)
        {
            query += $"$top={_top}&";
        }

        if (!string.IsNullOrEmpty(_expand))
        {
            query += $"$expand={_expand}&";
        }

        return query.TrimEnd('&');
    }
}
