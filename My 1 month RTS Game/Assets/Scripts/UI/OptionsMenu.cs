using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;
using TMPro;

using TheAshBot;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    private const string OPTIONS_DATA_PATH = "SavedData\\";
    private const string OPTIONS_DATA_NAME = "Option";


    [SerializeField] private GameObject window;


    [Header("Valume")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;


    [Header("Sceen Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int currentRefressRate;
    private List<Resolution> resolutionList;
    private List<Resolution> filteredResolutionList;


    [Header("Quality")]
    [SerializeField] private TMP_Dropdown graphicsQualityDropdown;


    private bool isOpen;


    private void Start()
    {
        DisplayResolutions();

        TryLoadOptions();
    }


    #region Buttons

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);

        Save();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        Save();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Save();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        isOpen = false;
        window.SetActive(isOpen);
    }

    public void Options()
    {
        isOpen = !isOpen;
        Time.timeScale = isOpen ? 0 : 1;
        window.SetActive(isOpen);
    }

    #endregion


    #region Save; Load

    public bool TryLoadOptions()
    {
        SaveOptionsData optionsData = SaveSystem.LoadJson<SaveOptionsData>(SaveSystem.RootSavePath.Resources, OPTIONS_DATA_PATH, OPTIONS_DATA_NAME);

        if (optionsData == null)
        {
            // Nothing was found
            this.Log("Nothing was found");
            return false;
        }

        resolutionDropdown.value = optionsData.filteredResolutionIndex;
        fullScreenToggle.isOn = optionsData.isFullScreened;
        graphicsQualityDropdown.value = optionsData.qualityIndex;
        volumeSlider.value = optionsData.volume;

        return true;
    }

    public void Save()
    {
        SaveOptionsData saveOptionsData = new SaveOptionsData()
        {
            isFullScreened = fullScreenToggle.isOn,
            filteredResolutionIndex = resolutionDropdown.value,
            volume = volumeSlider.value,
            qualityIndex = graphicsQualityDropdown.value,
        };

        SaveSystem.SaveJson(saveOptionsData, SaveSystem.RootSavePath.Resources, OPTIONS_DATA_PATH, OPTIONS_DATA_NAME, true);
    }

    #endregion

    private void DisplayResolutions()
    {
        resolutionList = Screen.resolutions.ToList();
        filteredResolutionList = new List<Resolution>();

        currentRefressRate = Screen.currentResolution.refreshRate;
        resolutionDropdown.ClearOptions();

        for (int i = 0; i < resolutionList.Count; i++)
        {
            if (resolutionList[i].refreshRate == currentRefressRate)
            {
                filteredResolutionList.Add(resolutionList[i]);
            }
        }

        List<string> screenResolutionStringList = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < filteredResolutionList.Count; i++)
        {
            string screenResolutionString = filteredResolutionList[i].width + " x " + filteredResolutionList[i].height;
            screenResolutionStringList.Add(screenResolutionString);

            if (filteredResolutionList[i].width == Screen.currentResolution.width && filteredResolutionList[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(screenResolutionStringList);
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }



    private class SaveOptionsData
    {
        public bool isFullScreened;
        public int filteredResolutionIndex;
        
        public int qualityIndex;

        public float volume;
    }


}
