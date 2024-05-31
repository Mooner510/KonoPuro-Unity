using System;
using System.Collections.Generic;
using _root.Script.Network;
using JetBrains.Annotations;

namespace _root.Script.Client
{
[Serializable]
public class GameCards
{
	public List<GameCard> cards;
}

[Serializable]
public class GameCard
{
	public string id;
	public string defaultCardType;
	public int    limit;
	public bool   dayTime;
}

[Serializable]
public class GameStudentCards
{
	public List<GameStudentCard> cards;
}

[Serializable]
public class GameStudentCard
{
	public string id;
	public string defaultCardType;
	public int    limit;
	public bool   dayTime;
}

[Serializable]
public class RawData
{
	public List<string> self;
	public List<string> other;
}

[CanBeNull]
[Serializable]
public class UpdatedData
{
	[CanBeNull] public string                                               id;
	[CanBeNull] public GameStudentCards                                     students;
	public             int?                                                 deckSize = null;
	[CanBeNull] public GameCards                                            heldCards;
	public             int?                                                 time = null;
	[CanBeNull] public GameCards                                            FieldCards;
	[CanBeNull] public Dictionary<MajorType, int>                           projects;
	[CanBeNull] public Dictionary<MajorType, List<int>>                     issues;
	public             bool?                                                sleep = null;
	[CanBeNull] public Dictionary<string, int>                              dateData;
	[CanBeNull] public Dictionary<MajorType, Dictionary<string, List<int>>> projectAdditionData;

	public static UpdatedData ConvertUpdatedData(List<string> data)
	{
		return new UpdatedData();
	}
}
}