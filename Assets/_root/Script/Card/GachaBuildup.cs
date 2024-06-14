using UnityEngine;
using UnityEngine.SceneManagement;

namespace _root.Script.Card
{
    public class GachaBuildup : MonoBehaviour
    {
        public void GachaBuildup1Ended()
        {
            SceneManager.LoadScene("GachaDirectingBuildupScene");
        }
        public void GachaBuildup2Ended()
        {
            SceneManager.LoadScene("GachaDirectingScene");
        }
    }
}
