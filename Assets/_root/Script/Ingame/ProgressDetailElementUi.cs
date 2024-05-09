using System;
using System.Collections;
using System.Collections.Generic;
using _root.Script.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressDetailElementUi : MonoBehaviour
{
    public MajorType type;

    private TextMeshProUGUI efficiencyText;
    private Slider          progressBar;
    private Image           majorIcon;

    private void Awake()
    {
        majorIcon      = GetComponentInChildren<Image>();
        efficiencyText = GetComponentInChildren<TextMeshProUGUI>();
        progressBar    = GetComponentInChildren<Slider>();
    }

}
