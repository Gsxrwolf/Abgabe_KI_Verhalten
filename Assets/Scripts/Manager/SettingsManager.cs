using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private TMP_Dropdown qualitDropDown;

    private void Start()
    {
        sliderMaster.value = GameManager.Instance.saveFile.volumeMaster;
        sliderMusic.value = GameManager.Instance.saveFile.volumeMusic;
        qualitDropDown.value = GameManager.Instance.saveFile.qualitySetting;
    }
    public void OnVolumeChange(string _name)
    {
        Slider slider = null;
        switch (_name)
        {
            case "Master":
                {
                    slider = sliderMaster;
                    break;
                }
            case "Music":
                {
                    slider = sliderMusic;
                    break;
                }
        }
        float value = slider.value;
        if (slider.value == slider.minValue)
        {
            value = -80;
        }
        AudioManager.Instance.SetVolume(_name, value);
    }
    public void OnQualityChange()
    {
        //0 = low
        //1 = medium
        //2 = high
        GameManager.Instance.saveFile.qualitySetting = qualitDropDown.value;
    }
}
