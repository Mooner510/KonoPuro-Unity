using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gachabox
{
    public class Draw : MonoBehaviour
    {
        public void Single()
        {
        
            Invoke("WaitforAni",5f);
        }

        public void Multiple()
        {
        
        }

        private void WaitforAni()
        {
            SceneManager.LoadScene("CardGatchaSingle");
        }
    
    }
}
