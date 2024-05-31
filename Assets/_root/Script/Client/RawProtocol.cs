using System;
using System.Text.Json.Serialization;

namespace _root.Script.Client
{
    [Serializable]
    public class RawProtocol
    {
        [JsonPropertyName("protocol")] public int protocol;

        [JsonPropertyName("data")] public string[] data;

        private RawProtocol()
        {
        }
        
        public static RawProtocol of(int protocol, params string[] data) => new()
                                                                            { protocol = protocol, data = data };
    }
}