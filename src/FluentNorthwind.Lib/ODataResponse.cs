using Newtonsoft.Json;

public class ODataResponse<T>
{
    [JsonProperty("@odata.context")]
    public string Context { get; set; }

    public List<T> Value { get; set; }

    // For single item responses
    [JsonProperty("item")]
    public T Item { get; set; }
}
