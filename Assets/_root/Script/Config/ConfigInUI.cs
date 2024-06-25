using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Manager;
using Config_Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigInUI : MonoBehaviour
{
    private GameObject Volume;
    private Slider VolumeSlider;
    //private SoundVolumeManager VolumeManager;
    private GameObject Light;
    private Slider LightSlider;
    private GameObject FPS_Limit;
    private Slider FPS_Slider;
    private TextMeshProUGUI FPS_Value;
    public bool isChange = false;

    private void Awake()
    {
        Volume = transform.GetChild(2).gameObject;
        VolumeSlider = Volume.GetComponent<Slider>();
        Light = transform.GetChild(3).gameObject;
        LightSlider = Light.GetComponent<Slider>();
        FPS_Limit = transform.GetChild(4).gameObject;
        FPS_Slider = FPS_Limit.GetComponent<Slider>();
        FPS_Value = FPS_Limit.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Init()
    {
        VolumeSlider.value = ConfigManager.ConfigData.SoundVolume;
        LightSlider.value = ConfigManager.ConfigData.Light;
        FPS_Slider.value = ConfigManager.ConfigData.FPS_Limit;
        FPS_Value.text = $"FPS : {(int)FPS_Slider.value}";
        isChange = false;
    }
    public void GetOff()
    {
        ConfigManager.ConfigData.SoundVolume = VolumeSlider.value;
        ConfigManager.ConfigData.Light = LightSlider.value;
        ConfigManager.ConfigData.FPS_Limit = (int)FPS_Slider.value;
        AudioManager.VolumeInitInstance();
        Settings.Save();
        if (isChange)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    //값 안저장
    public void DisAcceptChange()
    {
        Screen.brightness = ConfigManager.ConfigData.Light;
    }
    
    public void ChangeLight()
    {
        Screen.brightness = LightSlider.value;
        isChange = true;
    }
    public void ChangeVolume()
    {
        isChange = true;
    }

    public void ChangeFPS()
    {
        FPS_Value.text = $"FPS : {(int)FPS_Slider.value}";
        isChange = true;
    }
    public void JokingSystem()
    {
        LightSlider.value = 0.05f;
    }
}
