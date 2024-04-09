using System;
using System.Text.Json.Serialization;

namespace _root.Script.Client
{
    [Serializable]
    public class RawChat
    {
        [JsonPropertyName("protocol")] public int protocol;

        [JsonPropertyName("data")] public string data;
    }
}