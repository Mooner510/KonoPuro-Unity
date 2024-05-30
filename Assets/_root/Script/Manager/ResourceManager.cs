using System;
using System.Collections.Generic;
using _root.Script.Network;
using UnityEngine;

namespace _root.Script.Manager
{
[CreateAssetMenu(fileName = "ActiveSO", menuName = "Scriptable Object/ActiveSO", order = int.MaxValue)]
public class ActiveSO : ScriptableObject
{
	public int    useTime;
	public string skillName;
	public string description;
}

[CreateAssetMenu(fileName = "PassiveSO", menuName = "Scriptable Object/PassiveSO", order = int.MaxValue)]
public class PassiveSO : ScriptableObject
{
	public string skillName;
	public string description;
}

public class ResourceManager : MonoBehaviour
{
	private static readonly Dictionary<string, Sprite>      spriteDictionary  = new();
	private static readonly Dictionary<Tiers, ActiveSO>     activeDictionary  = new();
	private static readonly Dictionary<Passives, PassiveSO> passiveDictionary = new();

	public static Sprite GetSprite(string type)
	{
		if (type == null) return null;
		if (spriteDictionary.TryGetValue(type, out var data)) return data;
		var sprite = Resources.Load<Sprite>($"Card/DisplayData/Sprite/{type}");
		if (!sprite)
		{
			Debug.Log("Sprite Is Null");
			return null;
		}

		spriteDictionary.Add(type, sprite);
		return sprite;
	}

	public static ActiveSO GetActive(Tiers type)
	{
		if (activeDictionary.TryGetValue(type, out var data)) return data;
		var tier = Resources.Load<ActiveSO>($"Card/DisplayData/Active/{type}");
		activeDictionary.Add(type, tier);
		return tier;
	}

	public static PassiveSO GetPassive(Passives type)
	{
		if (passiveDictionary.TryGetValue(type, out var data)) return data;
		var sprite = Resources.Load<PassiveSO>($"Card/DisplayData/Passive/{type}");
		passiveDictionary.Add(type, sprite);
		return sprite;
	}

	public static void ClearAll()
	{
		ClearSprites();
		ClearActives();
		ClearPassives();
	}

	public static void ClearSprites()
	{
		spriteDictionary.Clear();
	}

	public static void ClearActives()
	{
		activeDictionary.Clear();
	}

	public static void ClearPassives()
	{
		passiveDictionary.Clear();
	}
}
}