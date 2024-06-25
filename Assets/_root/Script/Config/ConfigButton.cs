using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigButton : MonoBehaviour
{
    private GameObject Configs;
    private GameObject UserInfoPanel;
    private UserInfoPanel InfoScript;
    private void Start()
    {
        Configs = transform.GetChild(0).GetChild(0).gameObject;
        UserInfoPanel = FindObjectOfType<UserInfoPanel>().gameObject;
        InfoScript = UserInfoPanel.GetComponent<UserInfoPanel>();
        UserInfoPanel.SetActive(false);
        Configs.SetActive(false);
    }

    public void ConfigOn()
    {
        Configs.SetActive(true);
        Configs.GetComponent<ConfigInUI>().Init();
    }

    public void ConfigOff()
    {
        Configs.SetActive(false);
    }
    public void InfoOn()
    {
        UserInfoPanel.SetActive(true);
        InfoScript.Init();
    }
    public void InfoOff()
    {
        UserInfoPanel.SetActive(false);
    }
}
