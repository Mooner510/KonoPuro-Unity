using _root.Script.Data.Event.CardEvents;

namespace _root.Script.Manager
{
    public class AbilityManager
    {
        public delegate void OnCardDraw(CardDrawEvent @event);
        
        public delegate void OnAbilityUse(AbilityUseEvent @event);
    }
}