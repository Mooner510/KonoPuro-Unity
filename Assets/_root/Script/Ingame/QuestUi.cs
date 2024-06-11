using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUi : MonoBehaviour
{
    [SerializeField] private Vector3 GetOnPos;
    [SerializeField] private Vector3 GetOffPos;
    [SerializeField] private float Speed;
    [SerializeField] private float WaitTime;
    private bool GettingOn;
    public void GetON()
    {
        GettingOn = true;
        StartCoroutine(ToGetOn());
    }
    public void GetOff()
    {
        GettingOn = false;
        StartCoroutine(ToGetOff());
    }

    IEnumerator ToGetOn()
    {
        Vector3 NowPos = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        float elapsedtime = 0f;
        while (elapsedtime < WaitTime)
        {
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        elapsedtime = 0f;
        while (elapsedtime < Speed)
        {
            if (GettingOn)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                    Vector3.Lerp(NowPos, GetOnPos, elapsedtime / Speed);
                elapsedtime += Time.deltaTime;
                yield return null;
            }
            else
            {
                yield break;
            }
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = GetOnPos;
    }
    
    IEnumerator ToGetOff()
    {
        Vector3 NowPos = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        float elapsedtime = 0f;
        while (elapsedtime < Speed)
        {
            if (!GettingOn)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                    Vector3.Lerp(NowPos, GetOffPos, elapsedtime / Speed);
                elapsedtime += Time.deltaTime;
                yield return null;
            }
            else
            {
                yield break;
            }
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = GetOffPos;
    }
}
