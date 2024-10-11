using Newtonsoft.Json;

namespace FluentNorthwind.Lib;

public class ODataResponse<T>
{
    [JsonProperty("@odata.context")]
    public string Context { get; set; }

    public List<T> Value { get; set; }
}
