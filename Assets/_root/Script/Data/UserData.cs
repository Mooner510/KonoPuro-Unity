using System.Collections.Generic;
using _root.Script.Network;
using _root.Script.Utils.SingleTon;

namespace _root.Script.Data
{
    public class UserData: SingleTon<UserData>
    {
        public     DeckResponse        ActiveDeck;
        public PlayerCardResponses InventoryCards;

        public int gold = 1000;
    }
}