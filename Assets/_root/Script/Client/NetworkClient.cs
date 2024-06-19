using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using _root.Script.Data;
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
public partial class NetworkClient : SingleMono<NetworkClient>
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

	private static string roomId;

	public static void Init() => Instance._Init();

	private static Coroutine listenCoroutine;

	private void _Init()
	{
		if(listenCoroutine != null) StopCoroutine(listenCoroutine);
		StartCoroutine(ListenEvent());		
	}

	public static void Send(params object[] data)
	{
		try
		{
			Debug.Log($"send to {roomId} :: {data[0]}");
			Debug.Log($"send data ::\n {JsonConvert.SerializeObject(data)}");
			Instance._client.EmitAsync(roomId, data);
		}
		catch (Exception e)
		{
			Debug.LogError(e);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Debug.Log("Awake");
	}

	private void Start()
	{
		Debug.Log("Start");
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
		              { Debug.Log($"{eventName}: Listened\n{response}");
		                try
		                {
			                var rawProtocols = response.GetValue<RawProtocols>();

			                foreach (var rawProtocol in rawProtocols.data)
			                {
				                try
				                {
					                switch (rawProtocol.protocol)
					                {
						                case 2:
							                roomId = (string)rawProtocol.data[0];
							                break;
						                case 200:
						                {
							                var rawData =
									                JsonConvert
											               .DeserializeObject<
												                RawData>(JsonConvert.SerializeObject(rawProtocol.data
														                         [0]));
							                var self  = UpdatedData.ConvertUpdatedData(rawData.self);
							                var other = UpdatedData.ConvertUpdatedData(rawData.other);

							                GameStatics.self   = self;
							                GameStatics.other  = other;
							                GameStatics.isTurn = rawData.turn;

							                CallEvent(ClientEvent.GameStarted, null);
							                break;
						                }
						                case 201:
							                CallEvent(ClientEvent.GameEnd, rawProtocol.data[0]);
							                break;
						                case 202:
							                var useCard = JsonConvert.DeserializeObject<GameCard>(rawProtocol.data[0]
									               .ToString());
							                CallEvent(ClientEvent.OtherCardUse, useCard);
							                break;
						                case 203:
							                
							                CallEvent(ClientEvent.OtherAbilityUse, null);
							                break;
						                case 204:
							                CallEvent(ClientEvent.NextDay, null);
							                break;
						                case 205:
						                {
							                var rawData =
									                JsonConvert.DeserializeObject<RawData>(rawProtocol.data[0]
											               .ToString());
							                var self  = UpdatedData.ConvertUpdatedData(rawData.self);
							                var other = UpdatedData.ConvertUpdatedData(rawData.other);
							                if (other?.heldCards?.cards != null)
								                other.heldCards.cards = other.heldCards.cards.Select(x => new GameCard()
												                 { id = x.id }).ToList();

							                GameStatics.isTurn = rawData.turn;

							                CallEvent(ClientEvent.DataUpdated, (self, other));
							                break;
						                }
					                }

					                Debug.Log($"Listen Success {rawProtocol.protocol}");
				                }
				                catch (Exception e)
				                {
					                Debug.LogError(e);
					                // ignored
				                }
			                }
		                }
		                catch (Exception e)
		                {
			                Debug.LogError(e);
			                // ignored
		                } });
		Debug.Log("Connecting..");
		await _client.ConnectAsync();
		Debug.Log("Connected!!!");
	}
}
}