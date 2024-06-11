using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestButton : MonoBehaviour
{
    private bool isActive;
    private RectTransform QuestTransform;
    [SerializeField] private GameObject QuestDataUI;
    [SerializeField] private Vector3 ButtonOffPos;
    [SerializeField] private Vector3 ButtonOnPos;
    [SerializeField] private Vector3 HoverOnPos;
    [SerializeField] private float ButtonMoveTime;
    private bool isActiveComplite = true;
    private void Start()
    {
        isActive = true;
    }

    public void Click()
    {
        if (isActive)
        {
            isActive = false;
            QuestDataUI.GetComponent<QuestUi>().GetON();
            StopCoroutine(ToOn());
            StartCoroutine(ToOff());
        }
        else
        {
            isActive = true;
            QuestDataUI.GetComponent<QuestUi>().GetOff();
            StopCoroutine(ToOff());
            StartCoroutine(ToOn());
        }
    }

    public void hoverOn()
    {
        if (isActiveComplite)
        {
            StartCoroutine(isHover());
        }
    }

    public void hoverOff()
    {
        if (isActiveComplite)
        {
            StartCoroutine(isDisHover());
        }
    }
    IEnumerator isHover()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            if (isActive)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                    Vector3.Lerp(ButtonOnPos, HoverOnPos, elapsedTime / 0.3f);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D = ButtonOnPos;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
    IEnumerator isDisHover()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            if (isActive)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                    Vector3.Lerp(HoverOnPos, ButtonOnPos, elapsedTime / 0.3f);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D = ButtonOnPos;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator ToOff()
    {
        float elapsedTime = 0f;
        Vector3 OnTransform = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        while (elapsedTime < ButtonMoveTime)
        {
            if (!isActive)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                    Vector3.Lerp(OnTransform, ButtonOffPos, elapsedTime / ButtonMoveTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
    IEnumerator ToOn()
    {
        float elapsedTime = 0f;
        Vector3 OnTransform = gameObject.GetComponent<RectTransform>().anchoredPosition3D;
        while (elapsedTime < ButtonMoveTime)
        {
            if (isActive)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition3D =
                    Vector3.Lerp(OnTransform, ButtonOnPos, elapsedTime / ButtonMoveTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isActiveComplite = true;
        yield break;
    }
}
