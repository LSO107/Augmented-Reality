using Newtonsoft.Json;

namespace Bindings
{
    public class ItemJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public string Link { get; set; }
    }
}