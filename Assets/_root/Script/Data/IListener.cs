using _root.Script.Data.Event.CardEvents;

namespace _root.Script.Data
{
    public interface IListener
    {
        public void OnCardDraw(CardDrawEvent @event);
    }
}