using System;
using System.Text.Json.Serialization;

namespace _root.Script.Client
{
    [Serializable]
    public class RawProtocol
    {
        [JsonPropertyName("protocol")] public int protocol;

        [JsonPropertyName("data")] public object[] data;

        private RawProtocol()
        {
        }

        public static RawProtocol of(int protocol, params object[] data) => new()
                                                                            { protocol = protocol, data = data };
    }
}