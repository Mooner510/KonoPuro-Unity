using _root.Script.Config;
using _root.Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public class InGameVolume : MonoBehaviour
{
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.value = ConfigManager.ConfigData.SoundVolume;
    }

    public void OnVolumeChanged()
    {
        ConfigManager.ConfigData.SoundVolume = _slider.value;
        AudioManager.VolumeInitInstance();
    }
}
