using System.ComponentModel;

namespace _root.Script.Data
{
    public class Ability
    {
        public AbilityType type;
        public CancelEventHandler eventHandler;
    }

    public enum AbilityType
    {
        Passive,
        Ability
    }
}