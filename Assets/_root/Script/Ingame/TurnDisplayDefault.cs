using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnDisplayDefault : MonoBehaviour
{
    public TextMeshProUGUI TurnText;
    private static TurnDisplayDefault Instance;
    
    public void Start() {
        Instance = this;
    }
    
    public static TurnDisplayDefault getInstance() {
        return Instance;
    }
    
    public void TurnChange(bool isTurn)
    {
        TurnText.text = isTurn ? "My Turn" : "Other Turn";
    }
}