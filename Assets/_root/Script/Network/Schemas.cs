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

public enum Passives
{
	ParallelProcess,
	IdeaDay,
	Overload,
	APIRequired,
	Novelist,
	ProNovelist,
	TimeSaving,
	MusicPlay,
	Music,
	InfinityMusic,
	RepeatMusic,
	MultiDevelop,
	FastAction,
	NightCoding,
	BlazePassion,
	Mastering,
	IssueCracker,
	Destore,
	Brocker
}

public enum Tiers
{
	Designer,
	Frontend,
	Backend,
	iOS,
	Android,
	MultiMajor,
	SingleFocus,
	IssueComplete,
	SoleDesigner,
	GreatDesigner,
	IssueTracker,
	AutonomyStudy,
	EasyFirst,
	CustomNovelist,
	AddBeat,
	BeatAddFE,
	BeatAddBE,
	MusicFocus,
	DJ,
	RegularMeeting,
	InfinityPassion,
	DoItWithTime,
	Reverse,
	Cooperation,
	Disturbance,
	Reverse2
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
public class PlayerCardResponse
{
	public string          id;
	public List<MajorType> cardGroups;
	public int             tier;
	public CardType        type;
	public List<Passives>  passives;
	public List<Tiers>     tiers;
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
public class DefaultDataResponse
{
	public string   id;
	public CardType type;
	public int      tier;
}

[Serializable]
public class StudentDataResponse
{
	public string          title;
	public CardType        type;
	public string          description;
	public List<MajorType> cardGroups;
	public List<Passives>  defaultPassives;
	public List<Tiers>     tier1;
	public List<Tiers>     tier2;
	public List<Passives>  tier3;
	public List<Tiers>     tier4;
}

[Serializable]
public class CardDataResponses
{
	public List<StudentDataResponse> students;
	public List<DefaultDataResponse> cards;
}

[Serializable]
public class DeckResponse
{
	public string       deckId;
	public List<string> deck;
}

[Serializable]
public class ApplyDeckRequest
{
	public string       activeDeckId;
	public List<string> addition;
	public List<string> deletion;
}
}