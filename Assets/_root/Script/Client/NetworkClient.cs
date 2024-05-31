using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using _root.Script.Data;
using _root.Script.Manager;
using _root.Script.Network;
using _root.Script.Utils.SingleTon;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocketIO.Core;
using SocketIO.Serializer.NewtonsoftJson;
using SocketIOClient;
using SocketIOClient.Transport;
using UnityEngine;

namespace _root.Script.Client
{
public class NetworkClient : SingleMono<NetworkClient>
{
	public bool   localHost;
	public bool   security;
	public string host;
	public int    port;
	public bool   ignorePort;
	public string path;

	private SocketIOClient.SocketIO _client;
	private bool                    _connected;
	private NetworkStream           _stream;
	private Thread                  _thread;

	public static Action                           onMatched;
	public static Action                           gameStarted;
	public static Action<UpdatedData, UpdatedData> updateData;

	private static string roomId;

	public static void Send(params object[] data)
	{
		Instance._client.EmitAsync(roomId, data);
	}

	private void Start()
	{
		if (localHost)
		{
			host = IPAddressor.GetLocalIP();
		}

		if (host is not
		    { Length: > 0 })
		{
			host = "socket.dsm-dongpo.com";
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
		var uri = new Uri($"{(security ? "https" : "http")}://{host}{(ignorePort ? "" : $":{port}")}{path}");
		Debug.Log($"Ready to Listen: {uri}");
		_client = new SocketIOClient.SocketIO(uri, new SocketIOOptions
		                                           { Transport = TransportProtocol.WebSocket,
		                                             // EIO       = EngineIO.V4,
		                                             ExtraHeaders = new Dictionary<string, string>
		                                                            { ["Authorization"] = Networking.AccessToken } });
		_client.Serializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings
		                                                  { ContractResolver = new DefaultContractResolver
		                                                                       { NamingStrategy =
				                                                                       new
						                                                                       CamelCaseNamingStrategy() } });
		_client.OnConnected        += (sender, args) => { Debug.Log($"Connected, {args}"); };
		_client.OnError            += (sender, s) => { Debug.LogError($"Error: {sender}, {s}"); };
		_client.OnReconnected      += (sender, i) => { Debug.Log($"Reconnected {i}"); };
		_client.OnReconnectFailed  += (sender, args) => { Debug.Log($"Reconnected Failed, {args}"); };
		_client.OnReconnectError   += (sender, args) => { Debug.Log($"Reconnected Error, {args}"); };
		_client.OnReconnectAttempt += (sender, args) => { Debug.Log($"Reconnected Attempt, {args}"); };
		// _client.On("chat", response =>
		// {
		//     response.GetValue<RawChat>()
		// });
		_client.OnAny((eventName, response) =>
		              { var rawProtocol = response.GetValue<RawProtocol>();

		                if (rawProtocol.protocol == 2)
		                {
			                roomId = rawProtocol.data[0];
			                onMatched.Invoke();
		                }
		                else if (rawProtocol.protocol == 200)
		                {
			                var rawData = JsonConvert.DeserializeObject<RawData>(rawProtocol.data[0]);
			                var self    = UpdatedData.ConvertUpdatedData(rawData.self);
			                var other   = UpdatedData.ConvertUpdatedData(rawData.other);

			                GameStatics.self  = self;
			                GameStatics.other = other;

			                gameStarted.Invoke();
		                }
		                else if (rawProtocol.protocol == 206)
		                {
			                var rawData = JsonConvert.DeserializeObject<RawData>(rawProtocol.data[0]);
			                var self    = UpdatedData.ConvertUpdatedData(rawData.self);
			                var other   = UpdatedData.ConvertUpdatedData(rawData.other);

			                updateData.Invoke(self, other);
		                }

		                Debug.Log($"{eventName}: {JsonConvert.SerializeObject(rawProtocol)}"); });
		Debug.Log("Connecting..");
		await _client.ConnectAsync();
		Debug.Log("Connected!!!");
	}
}
}