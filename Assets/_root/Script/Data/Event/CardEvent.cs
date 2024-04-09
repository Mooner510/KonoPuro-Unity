namespace _root.Script.Data.Event
{
    public class CardEvent
    {
        private readonly Card _card;

        public CardEvent(Card card)
        {
            _card = card;
        }

        public Card GetCard()
        {
            return _card;
        }
    }
}