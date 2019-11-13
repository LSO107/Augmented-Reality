using Newtonsoft.Json;

namespace Bindings
{
    public class ItemJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public string Link { get; set; }

        [JsonProperty(Required = Required.Always)]
        public ImageJsonBinding Image { get; set; }
    }

    public class ImageJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public string ContextLink { get; set; }
    }
}