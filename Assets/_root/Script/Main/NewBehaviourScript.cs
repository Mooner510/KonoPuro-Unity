using UnityEngine;
using UnityEngine.SceneManagement;

namespace _root.Script.Main
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public void doGacha()
        {
            SceneManager.LoadScene("GachaScene");
        }
    }
}