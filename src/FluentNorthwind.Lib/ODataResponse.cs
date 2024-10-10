using Newtonsoft.Json;

public class ODataResponse<T>
{
    [JsonProperty("value")]
    public List<T> Value { get; set; }
}
