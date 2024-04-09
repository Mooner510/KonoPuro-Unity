namespace _root.Script.Data.Event
{
    public class CardsEvent
    {
        private readonly Card[] _card;

        public CardsEvent(Card[] card)
        {
            _card = card;
        }

        public Card[] GetCards()
        {
            return _card;
        }
    }
}