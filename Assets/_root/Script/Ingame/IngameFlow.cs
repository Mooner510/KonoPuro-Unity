using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _root.Script.Client;
using _root.Script.Data.Event.CardEvents;
using _root.Script.Network;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IngameFlow : MonoBehaviour
{
	private const int dDay = 15;
	private       int day;

	private readonly GameStatus self  = new ();
	private readonly GameStatus other = new ();

	private IngameUi ui;

	public static bool myTurn;

	private void Awake()
	{
		ui = FindObjectOfType<IngameUi>();
		NetworkClient.AddEvent(203, _=>NextDay());
		NetworkClient.AddEvent(208, ProjectUpdate);
		NetworkClient.AddEvent(207, FieldUpdate);
		NetworkClient.AddEvent(206, HeldUpdate);
		NetworkClient.AddEvent(205, DrawCard);
		NetworkClient.AddEvent(209, SetTurn);
	}

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		day       = 1;
		self.Init();
		other.Init();
		ui.Init(dDay, day, self, other);
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
	public List<DetailProgressInfo> detailProgresses;

	public void Init()
	{
		time          = 24;
		totalProgress = 0;

		var gameMajors = ((MajorType[])Enum.GetValues(typeof(MajorType))).ToList();
		
		detailProgresses = new List<DetailProgressInfo>();
		foreach (var major in gameMajors)
			detailProgresses.Add(new DetailProgressInfo {major = major});
	}
}

public class DetailProgressInfo
{
	public MajorType major;
	public float       progress = 0;
	public float       efficiency = 100;
}