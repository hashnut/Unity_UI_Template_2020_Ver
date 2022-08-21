using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("[General Setting]")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController _menuController;

    [Header("[Volume Setting]")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("[Brightness Setting]")]
    [SerializeField] private Slider _brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("[Quality Level Setting]")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("[FullScreen Setting]")]
    [SerializeField] private Toggle _fullScreenToggle; 

    [Header("[Sensitivity Setting]")]
    [SerializeField] private TMP_Text _controllerSenTextValue = null;
    [SerializeField] private Slider _controllerSenSlider = null;

    [Header("[Invert Y Setting]")]
    [SerializeField] private Toggle _invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                _menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                _qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    _fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    _fullScreenToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                int localbrightness = PlayerPrefs.GetInt("masterBrightness");

                brightnessTextValue.text = localbrightness.ToString("0.0");
                _brightnessSlider.value = localbrightness;
                // Change the brightnesss
            }

            if (PlayerPrefs.HasKey("masterSen"))
            {
                int localSensitivity = PlayerPrefs.GetInt("masterSen");

                _controllerSenTextValue.text = localSensitivity.ToString("0");
                _controllerSenSlider.value = localSensitivity;
                _menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                if (PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    _invertYToggle.isOn = true;
                }
                else
                {
                    _invertYToggle.isOn = false;
                }
            }
        }
    }
}
