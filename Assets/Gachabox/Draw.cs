using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Draw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
