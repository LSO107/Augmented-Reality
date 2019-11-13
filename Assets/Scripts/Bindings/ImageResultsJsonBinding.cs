using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bindings
{
    public class ImageResultsJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public ItemJsonBinding[] Items;
    }
}