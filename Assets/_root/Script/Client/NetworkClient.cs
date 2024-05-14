using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using _root.Script.Manager;
using _root.Script.Network;
using _root.Script.Utils.SingleTon;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocketIO.Serializer.NewtonsoftJson;
using SocketIOClient;
using SocketIOClient.Transport;
using UnityEngine;

namespace _root.Script.Client
{
    public class NetworkClient : SingleMono<NetworkClient>
    {
        public bool autoHost;
        public bool security;
        public string host;
        public int port;
        public bool ignorePort;
        public string path;

        private SocketIOClient.SocketIO _client;
        private bool _connected;
        private NetworkStream _stream;
        private Thread _thread;

        private static readonly Dictionary<int, Action<Dictionary<string, object>>> actions = new ();

        public static void AddEvent(int protocol, Action<Dictionary<string, object>> action)
        {
            actions.TryAdd(protocol, _ => { });
            actions[protocol] += action;
        }

        public static void Send(params object[] data)
        {
            Instance._client.EmitAsync("chat", data);
        }

        private void Start()
        {
            if (autoHost)
            {
                host = IPAddressor.GetLocalIP();
            }

            Listen();

            // _thread = new Thread(Listen)
            // {
            //     IsBackground = true
            // };
            // _thread.Start();
        }

        private void OnDestroy()
        {
            _client?.DisconnectAsync();
        }

        private void OnApplicationQuit()
        {
            _client?.DisconnectAsync();
        }

        private async void Listen()
        {
            Debug.Log("Ready to Listen");
            
            var uri = new Uri($"{(security ? "https" : "http")}://{host}{(ignorePort ? "" : $":{port}")}{path}");
            Debug.Log(uri);
            _client = new SocketIOClient.SocketIO(uri, new SocketIOOptions
            {
                Transport = TransportProtocol.WebSocket,
                ExtraHeaders = new Dictionary<string, string>
                {
                    ["Authorization"] = Networking.AccessToken
                }
            });
            _client.Serializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            _client.OnConnected += (sender, args) => { Debug.Log($"Connected, {args}"); };
            _client.OnError += (sender, s) => { Debug.LogError($"Error: {sender}, {s}"); };
            _client.OnReconnected += (sender, i) => { Debug.Log($"Reconnected {i}"); };
            _client.OnReconnectFailed += (sender, args) => { Debug.Log($"Reconnected Failed, {args}"); };
            _client.OnReconnectError += (sender, args) => { Debug.Log($"Reconnected Error, {args}"); };
            _client.OnReconnectAttempt += (sender, args) => { Debug.Log($"Reconnected Attempt, {args}"); };
            // _client.On("chat", response =>
            // {
            //     response.GetValue<RawChat>()
            // });
            _client.OnAny((eventName, response) =>
            {
                var rawData = response.GetValue<RawData>();
                if (actions.TryGetValue(rawData.protocol, out var action))
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawData.data[0]);
                     action.Invoke(data);
                }
                Debug.Log($"{eventName}: {JsonConvert.SerializeObject(rawData)}");
            });
            Debug.Log("Connecting..");
            await _client.ConnectAsync();
            Debug.Log("Connected!!!");
        }
    }
}
