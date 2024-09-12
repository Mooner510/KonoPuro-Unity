using System.Collections;
using System.Collections.Generic;
using _root.Script.Main;
using UnityEngine;
using UnityEngine.UI;

public class BannerChanger : MonoBehaviour
{
    [SerializeField] private Image _bannerChange;
    [SerializeField] private Sprite _frontEnd;
    [SerializeField] private Sprite _backEnd;
    
    
    public void ChageToFront()
    {
        _bannerChange.sprite = _frontEnd;
    }

    public void ChangeToBack()
    {
        _bannerChange.sprite = _backEnd;
    }
}
