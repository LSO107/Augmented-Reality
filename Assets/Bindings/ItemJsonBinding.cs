using Newtonsoft.Json;

public class ItemJsonBinding
{
    [JsonProperty(Required = Required.Always)]
    public string Link { get; set; }
}