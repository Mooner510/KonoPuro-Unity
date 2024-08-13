using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _root.Script.Ingame
{
    public class GameEndUi : MonoBehaviour
    {
        private TextMeshProUGUI winText;

        private void Awake()
        {
            winText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            Set(false, null);
        }

        public void Set(bool active, string info)
        {
            gameObject.SetActive(active);
            if (!active) return;

            winText.text = info switch
            {
                "DRAW" => "무승부",
                "WIN" => "승리!",
                "LOSE" => "패배...",
                "WIN_SURRENDER" => "승리! (상대의 항복)",
                "LOSE_SURRENDER" => "항복...",
                _ => winText.text
            };
        }

        public void ReturnToMain()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}