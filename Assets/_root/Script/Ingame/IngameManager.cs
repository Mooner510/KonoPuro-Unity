using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Card;
using _root.Script.Client;
using _root.Script.Data;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.Playables;

public class IngameManager : MonoBehaviour
{
	private PlayableDirector director;
	private PlayerActivity   activity;

	[SerializeField] private PlayableAsset start;

	private int day;

	private readonly GameStatus selfStats  = new();
	private readonly GameStatus otherStats = new();

	public List<GameStudentCard> selfStudents  = new();
	public List<GameStudentCard> otherStudents = new();

	private DrawDeck selfDeck;
	private DrawDeck otherDeck;

	private FieldSetter selfField;
	private FieldSetter otherField;

	private IngameUi ui;

	public static bool myTurn;

	//TODO: 실험용 삭제필요
	[SerializeField] private bool spriteDebug;

	private void Awake()
	{
		var decks = FindObjectsOfType<DrawDeck>();
		selfDeck  = decks.First(x => x.isMine);
		otherDeck = decks.First(x => !x.isMine);

		var fields = FindObjectsOfType<FieldSetter>();
		selfField  = fields.First(x => x.isMine);
		otherField = fields.First(x => !x.isMine);

		director = GetComponent<PlayableDirector>();
		activity = GetComponent<PlayerActivity>();

		ui = FindObjectOfType<IngameUi>();

		var lights = FindObjectsOfType<Light>();
		foreach (var areaLight in lights)
			areaLight.intensity = 0;

		if (!spriteDebug)
		{
			var sprites = FindObjectsOfType<SpriteRenderer>().Where(x => !x.GetComponent<Card>());
			foreach (var spriteRenderer in sprites)
				Destroy(spriteRenderer);
		}

		FindObjectsOfType<Canvas>().First(x => x.gameObject.name == "Field Canvas").enabled = false;

		NetworkClient.updateData  += UpdateData;
	}

	private void Start()
	{
		//TODO: 실험용 삭제 필요
		List<GameStudentCard> students = new()
		                                 { new(), new(), new(), new() };
		selfStudents  = students;
		otherStudents = students;

		StartCoroutine(StartFlow(new()
		                         { new(), new(), new(), new(), new() }, 5));


		//TODO: 위의 실험용 삭제 시 사용
		// GameStart();
	}

	private void GameStart()
	{
		var selfStudent = GameStatics.self.student;
		var otherStudent = GameStatics.other.student;
		if (selfStudent == null || otherStudent == null)
		{
			Debug.LogError("game start student cards data is null");
			return;
		}
		selfStudents = selfStudent.cards;
		selfStudents = selfStudent.cards;
		var selfHeldCards  = GameStatics.self.heldCards;
		var otherHeldCards = GameStatics.other.heldCards;
		if (selfHeldCards == null || otherHeldCards == null)
		{
			Debug.LogError("game start held cards data is null");
			return;
		}
		StartCoroutine(StartFlow(selfHeldCards.cards, otherHeldCards.cards.Count));
	}

	private IEnumerator StartFlow(List<GameCard> selfDraws, int otherDrawCount)
	{
		day = 1;
		selfStats.Init();
		otherStats.Init();

		ui.Init(day, selfStats, otherStats);

		yield return new WaitForSeconds(1f);

		director.playableAsset = start;
		director.Play();

		yield return new WaitForSeconds(1f);

		StartCoroutine(SetStudent(true));
		StartCoroutine(SetStudent(false));

		yield return new WaitForSeconds(1f);

		selfDeck.Init();
		otherDeck.Init();

		//TODO: 처음 시작할 때 카드 드로우 - 지금은 임시용 변경 필요
		selfDeck.DrawCards(activity.AddHandCard, false, selfDraws);
		otherDeck.DrawCards(activity.AddHandCard, false, otherDrawCount);

		yield return new WaitForSeconds(3f);

		ui.TurnSet(false);

		activity.SetActive(true);
	}

	private IEnumerator SetStudent(bool isMine)
	{
		var students = isMine ? selfStudents : otherStudents;
		var field    = isMine ? otherField : selfField;

		foreach (var student in students)
		{
			field.AddNewCard(student);
			yield return new WaitForSeconds(0.25f);
		}
	}

	private void UpdateData(UpdatedData self, UpdatedData other)
	{
	}

	private void NextDay()
	{
		day++;
		ui.DayChange(day);
	}

	private void ProjectUpdate(Dictionary<string, object> dictionary)
	{
	}

	private void FieldUpdate(Dictionary<string, object> dictionary)
	{
	}

	private void HeldUpdate(Dictionary<string, object> dictionary)
	{
	}

	private void DrawCard(Dictionary<string, object> dictionary)
	{
	}

	private void SetTurn(Dictionary<string, object> dictionary)
	{
	}
}

public class GameStatus
{
	public int                      time;
	public float                    totalProgress;
	public List<DetailProgressInfo> detailProgresses = new();

	public void Init()
	{
		time          = 24;
		totalProgress = 0;

		var gameMajors = ((MajorType[])Enum.GetValues(typeof(MajorType))).ToList();

		detailProgresses = new List<DetailProgressInfo>();
		foreach (var major in gameMajors)
			detailProgresses.Add(new DetailProgressInfo
			                     { major = major });
	}
}

public class DetailProgressInfo
{
	public MajorType major;
	public float     progress   = 0;
	public float     efficiency = 100;
}