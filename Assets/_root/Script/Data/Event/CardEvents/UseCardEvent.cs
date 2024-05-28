using System;

namespace _root.Script.Data.Event.CardEvents
{
[Serializable]
public class UseCardEvent : Event
{
	public readonly int    protocol = 103;
	public string id;
}
}