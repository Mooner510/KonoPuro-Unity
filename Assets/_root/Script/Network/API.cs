namespace _root.Script.Network
{
    public static class API
    {
        public static Networking.Post<Void> SignUp(SignUpRequest req)
            => new("/api/auth/sign-up", req);

        public static Networking.Post<TokenResponse> SignIn(SignInRequest req)
            => new("/api/auth/sign-in", req);

        public static Networking.Put<Void> PasswordChange(PasswordChangeRequest req)
            => new("/api/auth/password", req);

        public static Networking.Put<Void> IdChange(IdChangeRequest req)
            => new("/api/auth/id", req);

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

        public static Networking.Get<GatchaLogDataResponse> GatchaLog()
            => new("/api/gatcha/log");

        public static Networking.Get<GatchaResponses> GatchaList()
            => new("/api/gatcha/list");

        public static Networking.Get<CardDataResponse> GetCard(string name, string id)
        {
            var get = new Networking.Get<CardDataResponse>("/api/card");
            get.AddParam("name", name);
            get.AddParam("id", id);
            return get;
        }

        public static Networking.Get<CardDataResponses> GetCardAll()
            => new("/api/card/all");

        public static Networking.Post<Void> Match()
            => new("/api/game/match", null);

        public static Networking.Get<PlayerCardResponses> GetInventoryCardAll()
            => new("/api/inventory");

        public static Networking.Get<DeckResponse> GetActiveDeck()
            => new("/api/inventory/active");

        public static Networking.Post<Void> ApplyDeck(ApplyDeckRequest req)
           => new("/api/inventory/apply", req);
    }
}