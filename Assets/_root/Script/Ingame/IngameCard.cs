using _root.Script.Network;
using UnityEngine;

namespace _root.Script.Ingame
{

public enum IngameCardType
{
	Student,
	Held,
	Field
}

public class IngameCard : MonoBehaviour
{
	public IngameCardType type;
	
	[HideInInspector]
	public PlayerCardResponse card;
}
}