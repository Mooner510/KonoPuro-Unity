using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ProgressDetailElementUi : MonoBehaviour
{
    private MajorType type;

    private Image           majorIcon;
    private Slider          progressBar;
    private TextMeshProUGUI progressText;
    private TextMeshProUGUI efficiencyText;

    private void Awake()
    {
        majorIcon      = GetComponentInChildren<Image>();
        progressBar    = GetComponentInChildren<Slider>();
        efficiencyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(DetailProgressInfo info)
    {
        //TODO: 전공에 맞는 아이콘 불러오기 & 적용

        progressBar.value   = info.progress;
        progressText?.SetText($"{info.progress}pt");
        efficiencyText.SetText($"{info.efficiency}pt");
    }
}
