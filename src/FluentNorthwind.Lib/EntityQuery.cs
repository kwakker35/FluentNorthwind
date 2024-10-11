using System.Linq.Expressions;
using FluentNorthwind.Lib.Model;
using Newtonsoft.Json;

namespace FluentNorthwind.Lib;

public class EntityQuery<T>
{
    private readonly HttpClient _httpClient;
    private string _entitySetName;
    private string _filter;
    private string _orderBy;
    private int? _top;
    private string _expand;
    private string _select;
    private object _id = null;

    public EntityQuery(HttpClient httpClient, string entitySetName)
    {
        _httpClient = httpClient;
        _entitySetName = entitySetName;
    }

    public EntityQuery<T> WithId(int id)
    {
        _id = id;
        return this;
    }

    // Set string Id
    public EntityQuery<T> WithId(string id)
    {
        _id = id;
        return this;
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

    public EntityQuery<T> Select<TResult>(Expression<Func<T, TResult>> selector)
    {
        // Extract property names from the lambda expression
        var selectedProperties = new List<string>();

        if (selector.Body is NewExpression newExpression)
        {
            foreach (var argument in newExpression.Arguments)
            {
                if (argument is MemberExpression memberExpression)
                {
                    selectedProperties.Add(memberExpression.Member.Name);
                }
            }
        }
        else if (selector.Body is MemberExpression member)
        {
            selectedProperties.Add(member.Member.Name);
        }

        var selectedFields = string.Join(",", selectedProperties);

        // Append the $select parameter to the OData query
        _select = selectedFields;

        return this; // Return the current instance for method chaining
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

        // If the response has a value (list), return it; otherwise return the single item as a list
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
        if (_id != null)
        {
            // Handle numeric or string IDs
            if (_id is int || _id is long)
            {
                _entitySetName += $"({_id})";
            }
            else if (_id is string)
            {
                _entitySetName += $"('{_id}')";
            }
        }

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
