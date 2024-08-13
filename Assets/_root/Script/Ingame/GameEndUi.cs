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
                "DRAW" => "Draw",
                "WIN" => "You Win",
                "LOSE" => "You Lose",
                "WIN_SURRENDER" => "You Win By Surrender",
                "LOSE_SURRENDER" => "You Lose By Surrender",
                _ => winText.text
            };
        }

        public void ReturnToMain()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}