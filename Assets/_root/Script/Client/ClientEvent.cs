using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _root.Script.Client
{
public partial class NetworkClient
{
	private static readonly HashSet<ClientEvent>            events    = new();
	private static readonly Dictionary<ClientEvent, Action> delegates = new();

	public static void CallEvent(ClientEvent @event) => events.Add(@event);

	public static void DelegateEvent(ClientEvent @event, Action action) => delegates[@event] = action;

	public enum ClientEvent
	{
		GameStarted,
		DataUpdated,
		NextDay
	}

	private static IEnumerator ListenEvent(ClientEvent @event)
	{
		while (true)
		{
			yield return new WaitUntil(() => events.Remove(@event));
			if (delegates.TryGetValue(@event, out var action)) action();
		}
	}
}
}