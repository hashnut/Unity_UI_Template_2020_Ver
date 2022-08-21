using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("[Volume Setting]")]
    [SerializeField] private TMP_Text _volumeTextValue = null;
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private float _defaultVolume = 1.0f;

    [Header("[Gameplay Settings]")]
    [SerializeField] private TMP_Text _controllerSenTextValue = null;
    [SerializeField] private Slider _controllerSenSlider = null;
    [SerializeField] private int _defaultSen = 4;
    public int mainControllerSen = 4;


    [Header("[Toggle Settings]")]
    [SerializeField] private Toggle _invertYToggle = null;


    [Header("[Graphics Settings]")]
    [SerializeField] private Slider _brightnessSlider = null;
    [SerializeField] private TMP_Text _brightnessTextValue = null;
    [SerializeField] private float _defaultBrightness = 1.0f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private Toggle _fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;


    [Header("[Confirmation]")]
    [SerializeField] private GameObject _confirmationPrompt = null;

    [Header("[Levels To Load]")]
    public string newGameLevel;
    private string _levelToLoad;
    [SerializeField] private GameObject _noSavedGameDialog = null;

    [Header("[Resolution Dropdowns]")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }


    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            _levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(_levelToLoad);
        }
        else
        {
            _noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
        //PlayerPrefs.Save();
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        _controllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if (_invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            // invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            // Not invert
        }

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);

        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        _brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        // Change your brightness with your post processing or whatever it is

        PlayerPrefs.SetFloat("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }


    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            // Reset brightness value
            _brightnessSlider.value = _defaultBrightness;
            _brightnessTextValue.text = _defaultBrightness.ToString("0.0");

            _qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            _fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (MenuType == "Audio")
        {
            AudioListener.volume = _defaultVolume;
            _volumeSlider.value = _defaultVolume;
            _volumeTextValue.text = _defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            _controllerSenTextValue.text = _defaultSen.ToString("0");
            _controllerSenSlider.value = _defaultSen;
            mainControllerSen = _defaultSen;
            _invertYToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        _confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _confirmationPrompt.SetActive(false);
    }
}
 