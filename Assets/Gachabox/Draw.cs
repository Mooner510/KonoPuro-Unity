using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Draw : MonoBehaviour
{
    public void Single()
    {
        
        Invoke("WaitforAni",5f);
    }

    public void Multiple()
    {
        
    }

    void WaitforAni()
    {
        SceneManager.LoadScene("CardGatchaSingle");
    }
    
}
