using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class ImageResultsJsonBinding
{
    [JsonProperty(Required = Required.Always)]
    public ItemJsonBinding[] Items;
}