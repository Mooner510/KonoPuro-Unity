using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GachaInterAction : MonoBehaviour
{
    [SerializeField] private GameObject open;
    [SerializeField] private Animator boxani;
    [SerializeField] private GameObject uis;
    // Start is called before the first frame update
    void Start()
    {
        uis.SetActive(true);
        open.SetActive(false);
    }

    // Update is called once per frame
    public void Gacha1()
    {
        boxani.SetBool("gacha",true);
        uis.SetActive(false);
        open.SetActive(true);
    }
}
