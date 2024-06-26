using System;
using System.Collections.Generic;
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

        public static RawProtocol of(int protocol, params object[] data)
        {
            return new RawProtocol { protocol = protocol, data = data };
        }
    }

    [Serializable]
    public class RawProtocols
    {
        [JsonPropertyName("data")] public List<RawProtocol> data;

        private RawProtocols()
        {
        }
    }
}