using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;
using TMPro;

using TheAshBot;
using UnityEngine.Rendering;

public class OptionsMenu : MonoBehaviour
{


    [SerializeField] private GameObject window;


    [Header("Valume")]
    [SerializeField] private AudioMixer audioMixer;


    [Header("Sceen Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int currentRefressRate;
    private List<Resolution> resolutionList;
    private List<Resolution> filteredResolutionList;


    private bool isOpen;


    private void Start()
    {
        DisplayResolutions();
    }


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutionList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

}
