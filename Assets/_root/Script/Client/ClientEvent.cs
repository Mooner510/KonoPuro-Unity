using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _root.Script.Client
{
public partial class NetworkClient
{
	private static readonly Queue<Action> actions = new();
	private static readonly Action<object>[]      delegates = new Action<object>[Enum.GetValues(typeof(ClientEvent)).Length];

	private static void CallEvent(ClientEvent @event, object obj) => actions.Enqueue(()=>delegates[(int)@event](obj));

	public static void DelegateEvent(ClientEvent @event, Action<object> action)
	{
		delegates[(int) @event] = action;
	}

	public enum ClientEvent
	{
		GameStarted,
		GameEnd,
		OtherCardUse,
		OtherAbilityUse,
		NextDay,
		DataUpdated,
	}

	private static IEnumerator ListenEvent()
	{
		while (true)
		{
			yield return new WaitUntil(() => actions.Count > 0);
			actions.Dequeue()();
		}
	}
}
}