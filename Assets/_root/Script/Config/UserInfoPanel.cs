using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Data;
using _root.Script.Network;
using TMPro;
using UnityEngine;

public class UserInfoPanel : MonoBehaviour
{
    private GameObject UserID;
    private GameObject UserUUID;
    private Networking Net;

    private void Awake()
    {
        UserID = transform.GetChild(1).gameObject;
        Net = FindObjectOfType<Networking>();
    }

    public void Init()
    {
        UserID.GetComponent<TextMeshProUGUI>().text = $"ID fdf :  {Net.Password}";
    }
    
}
