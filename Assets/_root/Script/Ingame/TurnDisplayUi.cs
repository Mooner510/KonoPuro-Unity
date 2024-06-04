using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnDisplayUi : MonoBehaviour
{
    private TextMeshProUGUI turnNotify;

    private void Awake()
    {
        var tmps = GetComponentsInChildren<TextMeshProUGUI>();
        turnNotify = tmps[0];
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void TurnNotify(bool myTurn)
    {
        turnNotify.text = myTurn ? "My Turn" : "Other Turn";
        
        gameObject.SetActive(true);
        StartCoroutine(NotifySequence());
    }

    private IEnumerator NotifySequence()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
