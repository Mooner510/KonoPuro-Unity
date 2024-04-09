namespace _root.Script.Data.Event
{
    public class AbilityEvent : CardEvent
    {
        private readonly Ability _ability;

        public AbilityEvent(Card card, Ability ability) : base(card)
        {
            _ability = ability;
        }

        public Ability GetAbility()
        {
            return _ability;
        }
    }
}