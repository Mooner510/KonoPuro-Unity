using System;
using System.Collections.Generic;
using _root.Script.Client;
using _root.Script.Network;
using _root.Script.Utils.SingleTon;

namespace _root.Script.Data
{
public static class GameStatics
{
	public static int dDay = 15;

	public static int         gatchaOncePrice  = 100;
	public static int         gatchaMultiPrice = 999;

	public static int         deckCharacterCardRequired = 5;
	public static int         deckUseCardRequired       = 25;

	public static List<GatchaResponse> gatchaList;

	public static int         maxProgress = 1000;
	public static float       CalcTotalProgress(int sum) => (float)sum / (Enum.GetValues(typeof(MajorType)).Length * maxProgress);

	//동적 값들
	public static UpdatedData self;
	public static UpdatedData other;
	public static bool        isTurn;
	public static GameCard    useCard;
}
}