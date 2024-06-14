using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using static _root.Script.Data.GameStatics;

public class ProgressDetailElementUi : MonoBehaviour
{
    public MajorType type;

    private Image           majorIcon;
    private Slider          progressBar;
    private TextMeshProUGUI progressText;

    public int maxProgress;
    public int progress;

    private void Awake()
    {
        majorIcon      = GetComponentInChildren<Image>();
        progressBar    = GetComponentInChildren<Slider>();
        progressText = GetComponentInChildren<TextMeshProUGUI>();
        progress       = 0;
    }

    public void Init(Tuple<MajorType, int> info)
    {
        //TODO: 전공에 맞는 아이콘 불러오기 & 적용

        type              = info.Item1;
        maxProgress       = info.Item2;
        progressBar.value = 0;
        progressText.text = "0pt";
    }

    public void SetProgress(int _progress)
    {
        progress     = _progress;
        progressBar.value = Mathf.Clamp01((float)progress / maxProgress);
        progressText.text = $"{progress}pt";
    }

    public void AddProgress(int _progress)
    {
        progress          += _progress;
        SetProgress(progress);
    }
}
