using System;
using System.Collections.Generic;
using _root.Script.Network;
using UnityEngine;

namespace _root.Script.Manager
{
public class ResourceManager : MonoBehaviour
{
	private static readonly Dictionary<string, Sprite>        spriteDictionary = new ();

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

	public static void ClearSprites()
	{
		spriteDictionary.Clear();
	}
}
}