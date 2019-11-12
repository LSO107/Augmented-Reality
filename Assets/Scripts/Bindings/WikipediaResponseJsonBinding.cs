using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bindings
{
    public class WikipediaResponseJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public QueryJsonBinding Query { get; set; }
    }

    public class QueryJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public Dictionary<string, ResultExtractJsonBinding> Pages;

    }

    public class ResultExtractJsonBinding
    {
        [JsonProperty(Required = Required.Always)]
        public string Extract { get; set; }
    }
}
