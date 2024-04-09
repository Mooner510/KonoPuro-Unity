using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using _root.Script.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocketIO.Serializer.NewtonsoftJson;
using SocketIOClient;
using SocketIOClient.Transport;
using UnityEngine;

namespace _root.Script.Client
{
    public class NetworkClient : MonoBehaviour
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Q Key");
                var rawChat = new RawChat
                {
                    protocol = 1,
                    data = "룰루랄라!"
                };
                var serializedItems = _client.Serializer.Serialize(_client.Options.EIO, "chat", _client.Namespace,
                    new object[] { rawChat });
                foreach (var serializedItem in serializedItems) Debug.Log($"Data: {serializedItem.Text}");
                // Debug.Log($"FinalData: {serializedItems}");
                _client.EmitAsync("chat", rawChat);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("W Key");
                _client.EmitAsync("chat", new RawChat
                {
                    protocol = 5,
                    data = "korean"
                });
            }
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
                    ["Authorization"] = "test"
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
                Debug.Log($"{eventName}: {JsonConvert.SerializeObject(response.GetValue<RawChat>())}");
            });
            Debug.Log("Connecting..");
            await _client.ConnectAsync();
            Debug.Log("Connected!!!");
        }
    }
}