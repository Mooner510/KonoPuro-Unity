using System;
using System.Text.Json.Serialization;

namespace _root.Script.Client
{
    [Serializable]
    public class RawData
    {
        [JsonPropertyName("protocol")] public int protocol;

        [JsonPropertyName("data")] public string data;
    }
}