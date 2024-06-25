using System;

namespace _root.Script.Data.Event.CardEvents
{
    [Serializable]
    public class UseCardEvent : Event
    {
        public string id;
        public readonly int protocol = 103;
    }
}