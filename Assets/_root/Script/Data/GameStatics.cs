using System;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Network;

namespace _root.Script.Data
{
    public static class GameStatics
    {
        public static int dDay = 15;

        public static int gatchaOncePrice = 100;
        public static int gatchaMultiPrice = 999;

        public static int deckCharacterCardRequired = 5;
        public static int deckUseCardRequired = 25;

        public static Dictionary<Tiers, TierInfo> tierDictionary;
        public static Dictionary<Passives, PassiveInfo> passiveDictionary;
        public static Dictionary<string, DefaultCardInfo> defaultCardDictionary;
        public static Dictionary<string, StudentCardInfo> studentCardDictionary;

        public static List<GatchaResponse> gatchaList;

        public static readonly HashSet<Tuple<MajorType, int>> stageProjects = new()
        {
            new Tuple<MajorType, int>(MajorType.FrontEnd, 150),
            new Tuple<MajorType, int>(MajorType.Backend, 150),
            new Tuple<MajorType, int>(MajorType.Design, 80)
        };

        //동적 값
        public static UpdatedData self;
        public static UpdatedData other;
        public static bool isTurn;

        public static float CalcTotalProgress(int sum)
        {
            return (float)sum / stageProjects.Select(x => x.Item2).Sum();
        }
    }
}