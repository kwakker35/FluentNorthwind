using Newtonsoft.Json;

public class EntityQuery<T>
{
    private readonly HttpClient _httpClient;
    private string _entitySetName;
    private string _filter;
    private string _orderBy;
    private int? _top;
    private string _expand;
    private string _select;

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

    public EntityQuery<T> Select(string select)
    {
        _select = select;
        return this;
    }

    public async Task<IEnumerable<T>> ExecuteAsync()
    {
        var query = BuildQuery();
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }

        // Create JsonSerializerSettings to ignore null values
        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        // If the response has a value, return it; otherwise return the single item as a list
        if (json.Contains("\"value\":"))
        {
            // Deserialize as a collection
            var collectionResponse = JsonConvert.DeserializeObject<ODataResponse<T>>(
                json,
                settings
            );
            return collectionResponse.Value;
        }
        else
        {
            // Deserialize as a single item
            var singleItemResponse = JsonConvert.DeserializeObject<T>(json, settings);
            return new List<T> { singleItemResponse }; // Return as IEnumerable<T>
        }
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

        if (!string.IsNullOrEmpty(_select))
        {
            query += $"$select={System.Net.WebUtility.UrlEncode(_select)}";
        }

        return query.TrimEnd('&');
    }
}
