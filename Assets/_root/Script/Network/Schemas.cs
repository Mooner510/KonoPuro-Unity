using System;
using System.Collections.Generic;

namespace _root.Script.Network
{
    public enum MajorType
    {
        FrontEnd,
        Backend,
        Game,
        AI,
        Android,
        IOS,
        Embedded,
        DevOps,
        Design
    }

    public enum CardType
    {
        Student,
        Event,
        Tool,
        Activity
    }

    [Serializable]
    public class PasswordChangeRequest
    {
        public string id;
        public string password;
        public string newPassword;
    }

    [Serializable]
    public class IdChangeRequest
    {
        public string id;
        public string password;
        public string newId;
    }


    [Serializable]
    public class SignUpRequest
    {
        public string id;
        public string password;
        public string name;
    }


    [Serializable]
    public class SignInRequest
    {
        public string id;
        public string password;
    }


    [Serializable]
    public class TokenResponse
    {
        public string accessToken;
    }

    [Serializable]
    public class PassiveResponse
    {
        public string id;
        public string title;
        public string description;
    }

    [Serializable]
    public class TierResponse
    {
        public string id;
        public string title;
        public string description;
        public int    time;
    }

    [Serializable]
    public class PlayerCardResponse
    {
        public string                id;
        public string                title;
        public string                description;
        public List<MajorType>       cardGroups;
        public int                   tier;
        public CardType              type;
        public List<PassiveResponse> passives;
        public List<TierResponse>    tiers;
    }

    [Serializable]
    public class PlayerCardResponses
    {
        public List<PlayerCardResponse> cards;
    }

    [Serializable]
    public class GatchaLogResponse
    {
        public string cardName;
        public int    tier;
        public int    stack;
        public string dateTime;
    }

    [Serializable]
    public class GatchaLogDataResponse
    {
        public long                    total;
        public int                     filtered;
        public int                     stack3;
        public int                     stack4;
        public bool                    full3;
        public bool                    full4;
        public List<GatchaLogResponse> data;
    }

    [Serializable]
    public class GatchaResponse
    {
        public string    id;
        public string    name;
        public MajorType major;
        public string    startAt;
        public string    endAt;
    }

    [Serializable]
    public class GatchaResponses
    {
        public List<GatchaLogResponse> data;
    }

    [Serializable]
    public class CardDataResponse
    {
        public string                title;
        public string                description;
        public List<MajorType>       cardGroups;
        public CardType              type;
        public List<PassiveResponse> defaultPassives;
        public List<TierResponse>    tier2;
        public List<PassiveResponse> additionPassives;
        public List<TierResponse>    tier3;
    }

    [Serializable]
    public class CardDataResponses
    {
        public List<CardDataResponse> data;
    }

    [Serializable]
    public class DeckResponse
    {
        public string       deckId;
        public List<string> deck;
    }
    
    [Serializable]
    public class DeckCardRequest
    {
        public string deckId;
        public string cardId;
    }
        
    [Serializable]
    public class DeckCardRequests
    {
        public List<DeckCardRequest> data;
    }
}