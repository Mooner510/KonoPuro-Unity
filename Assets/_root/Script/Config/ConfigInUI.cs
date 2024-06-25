using System;
using System.Collections;
using System.Collections.Generic;
using Config_Manager;
using UnityEngine;
using UnityEngine.UI;

public class ConfigInUI : MonoBehaviour
{
    private GameObject Volume;
    private Slider VolumeSlider;
    private GameObject Light;
    private Slider LightSlider;
    private GameObject FPS_Limit;
    private Slider FPS_Slider;
    public bool isChange = false;

    private void Awake()
    {
        Volume = transform.GetChild(2).gameObject;
        VolumeSlider = Volume.GetComponent<Slider>();
        Light = transform.GetChild(3).gameObject;
        LightSlider = Volume.GetComponent<Slider>();
        FPS_Limit = transform.GetChild(4).gameObject;
        FPS_Slider = FPS_Limit.GetComponent<Slider>();
    }

    public void Init()
    {
        VolumeSlider.value = ConfigManager.ConfigData.SoundVolume;
        FPS_Slider.value = ConfigManager.ConfigData.FPS_Limit;
    }

    public void GetOff()
    {
        if (isChange)
        {
            //Todo: 저장 창 띄우기
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Change()
    {
        isChange = true;
    }
}
