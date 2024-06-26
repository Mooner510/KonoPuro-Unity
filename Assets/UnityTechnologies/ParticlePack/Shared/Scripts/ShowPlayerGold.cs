using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Data;
using TMPro;
using UnityEngine;

public class ShowPlayerGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI veiwgold;
    private bool switchbutton;
    // Start is called before the first frame update
    private void Start()
    {
        switchbutton = false;
    }

    public void OnClick()
    {
        veiwgold.text = UserData.Instance.gold.ToString();
    }
    
}
