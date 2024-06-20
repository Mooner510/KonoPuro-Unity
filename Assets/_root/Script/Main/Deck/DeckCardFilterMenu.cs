using System.Collections.Generic;
using System.Linq;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardFilterMenu : MonoBehaviour
{
	private GameObject studentFilter;
	private GameObject useCardFilter;

	private Toggle[] studentMajors;
	private Toggle[] studentTiers;

	private Toggle[] useCardTiers;

	public List<MajorType> SelectedMajors()
	{
		List<MajorType> selected = new List<MajorType>();
		for (var i = 0; i < studentMajors.Length; i++)
			if (studentMajors[i].isOn) selected.Add((MajorType)i);
		return selected;
	}

	public List<int> SelectedTier(bool character)
	{
		List<int> selected = new List<int>();
		if(character)
		{
			for (var i = 0; i < studentTiers.Length; i++)
				if (studentTiers[i].isOn) selected.Add(i);
		}
		else
		{
			for (var i = 0; i < useCardTiers.Length; i++)
				if(useCardTiers[i].isOn) selected.Add(i);
		}
		return selected;
	}

	// Start is called before the first frame update
	private void Awake()
	{
		studentFilter = transform.GetChild(0).gameObject;
		useCardFilter = transform.GetChild(1).gameObject;

		studentMajors = studentFilter.transform.GetChild(0).GetComponentsInChildren<Toggle>();
		for (var i = 0; i < studentMajors.Length; i++)
		{
			studentMajors[i].GetComponentInChildren<TextMeshProUGUI>().text = ((MajorType)i).ToString();
		}

		studentTiers = studentFilter.transform.GetChild(1).GetComponentsInChildren<Toggle>();
		for (var i = 3; i > -1; i--)
		{
			studentTiers[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = $"티어 {i}";
		}

		useCardTiers = useCardFilter.transform.GetChild(0).GetComponentsInChildren<Toggle>();
		for (var i = 3; i > -1; i--)
		{
			useCardTiers[i].GetComponentInChildren<TextMeshProUGUI>().text = $"티어 {i}";
		}
	}

	private void Start()
	{
		SetType(true);
		Show(false);
	}

	public void Init()
	{
		foreach (var toggle in studentMajors) toggle.isOn = true;
		foreach (var toggle in studentTiers) toggle.isOn  = true;
		foreach (var toggle in useCardTiers) toggle.isOn  = true;
	}

	public void SetType(bool character)
	{
		Init();
		studentFilter.SetActive(character);
		useCardFilter.SetActive(!character);
	}

	public void Show(bool show)
	{
		gameObject.SetActive(show);
	}
}