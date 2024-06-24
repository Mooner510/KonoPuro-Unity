using UnityEditor;
using UnityEngine;

namespace _root.Script.Card.Editor
{
    [CustomEditor(typeof(Card))]
    public class CardEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var card = (Card)target;

            if (!card.frontSide) card.frontSide = card.transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (!card.backSide) card.backSide = card.transform.GetChild(1).GetComponent<SpriteRenderer>();

            card.frontSide.sprite =
                (Sprite)EditorGUILayout.ObjectField("Front", card.frontSide.sprite, typeof(Sprite), false);
            // card.backSide.sprite = (Sprite)EditorGUILayout.ObjectField("Back", card.backSide.sprite, typeof(Sprite), false);
        }
    }
}