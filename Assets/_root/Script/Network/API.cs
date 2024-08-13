using System.Collections.Generic;

namespace _root.Script.Network
{
    public static class API
    {
        public static Networking.Post<Void> SignUp(SignUpRequest req)
        {
            return new Networking.Post<Void>("/api/auth/sign-up", req);
        }

        public static Networking.Post<TokenResponse> SignIn(SignInRequest req)
        {
            return new Networking.Post<TokenResponse>("/api/auth/sign-in", req);
        }

        public static Networking.Put<Void> PasswordChange(PasswordChangeRequest req)
        {
            return new Networking.Put<Void>("/api/auth/password", req);
        }

        public static Networking.Put<Void> IdChange(IdChangeRequest req)
        {
            return new Networking.Put<Void>("/api/auth/id", req);
        }

        public static Networking.Get<PlayerCardResponse> GatchaOnce(string gatchaId)
        {
            var get = new Networking.Get<PlayerCardResponse>("/api/gatcha/once");
            get.AddParam("gatchaId", gatchaId);
            return get;
        }

        public static Networking.Get<PlayerCardResponses> GatchaMulti(string gatchaId)
        {
            var get = new Networking.Get<PlayerCardResponses>("/api/gatcha/multi");
            get.AddParam("gatchaId", gatchaId);
            return get;
        }

        public static Networking.Get<GatchaLogDataResponse> GatchaLog(string tier)
        {
            var rep = new Networking.Get<GatchaLogDataResponse>("/api/gatcha/log");
            rep.AddParam("tier", tier);
            return rep;
        }

        public static Networking.Get<GatchaResponses> GatchaList()
        {
            return new Networking.Get<GatchaResponses>("/api/gatcha/list");
        }

        public static Networking.Get<DefaultDataResponse> GetDefaultCard(string id)
        {
            var get = new Networking.Get<DefaultDataResponse>("/api/card");
            get.AddParam("id", id);
            return get;
        }

        public static Networking.Get<StudentDataResponse> GetStudentCard(string id)
        {
            var get = new Networking.Get<StudentDataResponse>("/api/card");
            get.AddParam("id", id);
            return get;
        }

        public static Networking.Get<CardDataResponses> GetCardAll()
        {
            return new Networking.Get<CardDataResponses>("/api/card/all");
        }

        public static Networking.Post<Void> Match()
        {
            return new Networking.Post<Void>("/api/game/match", null);
        }

        public static Networking.Delete<Void> MatchCancel()
        {
            return new Networking.Delete<Void>("/api/game/match-cancel");
        }

        public static Networking.Get<PlayerCardResponses> GetInventoryCardAll()
        {
            return new Networking.Get<PlayerCardResponses>("/api/inventory");
        }

        public static Networking.Get<DeckResponse> GetActiveDeck()
        {
            return new Networking.Get<DeckResponse>("/api/inventory/active");
        }

        public static Networking.Post<Void> ApplyDeck(ApplyDeckRequest req)
        {
            return new Networking.Post<Void>("/api/inventory/apply", req);
        }

        public static Networking.Get<Dictionary<Tiers, TierInfo>> GetTiers()
        {
            return new Networking.Get<Dictionary<Tiers, TierInfo>>("/api/resource/tier");
        }

        public static Networking.Get<Dictionary<Passives, PassiveInfo>> GetPassives()
        {
            return new Networking.Get<Dictionary<Passives, PassiveInfo>>("/api/resource/passive");
        }

        public static Networking.Get<Dictionary<string, DefaultCardInfo>> GetDefaultCards()
        {
            return new Networking.Get<Dictionary<string, DefaultCardInfo>>("/api/resource/default-card");
        }

        public static Networking.Get<Dictionary<string, StudentCardInfo>> GetStudentCards()
        {
            return new Networking.Get<Dictionary<string, StudentCardInfo>>("/api/resource/student-card");
        }

        public static Networking.Get<GoldResponse> GetGold()
        {
            return new Networking.Get<GoldResponse>("/api/inventory/gold");
        }
        
        public static Networking.Get<Version> GetVersion()
        {
            return new Networking.Get<Version>("/api/resource/version");
        }
    }
}