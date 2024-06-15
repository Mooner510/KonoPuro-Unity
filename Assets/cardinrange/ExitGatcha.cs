using System.Collections;
using System.Collections.Generic;
using cardinrange;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitGatcha : MonoBehaviour
{
    public Transform cards;
    public void Exitgatcha()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void SkipGatcha()
    {
        gameObject.SetActive(false);
        foreach (var c in cards.GetComponentsInChildren<CardinrandomRange>()) c.OnMouseDown();
    }
}
