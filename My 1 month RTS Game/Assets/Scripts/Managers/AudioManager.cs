using System;
using System.Collections.Generic;

using TheAshBot;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{


    public static AudioManager Instance
    {
        get;
        private set;
    }


    public event EventHandler OnAudioClipFinished;


    [SerializeField] private List<AudioClip> mainMenuAudioClipList;
    [SerializeField] private List<AudioClip> loadingAudioClipList;
    [SerializeField] private List<AudioClip> inGameAudioClipList;


    private AudioSource audioSource;
    private AudioClip currentAudioClip;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            this.LogError("There are more than one \"AudioManager\" objects. This should never happen");
        }
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        GertAndPlayAudioCLip();

        OnAudioClipFinished += AudioManager_OnAudioClipFinished;
        SceneLoader.Instance.OnSceneLoaded += SceneLoader_OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GertAndPlayAudioCLip();
    }

    private void AudioManager_OnAudioClipFinished(object sender, EventArgs eventArgs)
    {
        GertAndPlayAudioCLip();
    }

    private void SceneLoader_OnSceneLoaded(object sender, SceneLoader.OnSceneLoadedEventArgs eventArgs)
    {
        GertAndPlayAudioCLip();
    }

    private void OnApplicationQuit()
    {
        OnAudioClipFinished = null;
    }

    private void GertAndPlayAudioCLip()
    {
        currentAudioClip = GetAudioClipForCurrentScene();
        audioSource.clip = currentAudioClip;
        audioSource.Play();

        WiatOntillTheMuscHasStoped();
    }

    private AudioClip GetAudioClipForCurrentScene()
    {
        if (SceneLoader.Instance.scene == SceneLoader.Scenes.MainMenu)
        {
            return mainMenuAudioClipList[UnityEngine.Random.Range(0, mainMenuAudioClipList.Count - 1)];
        }
        else if (SceneLoader.Instance.scene == SceneLoader.Scenes.Loading)
        {
            return loadingAudioClipList[UnityEngine.Random.Range(0, loadingAudioClipList.Count - 1)];
        }
        else if (SceneLoader.Instance.scene == SceneLoader.Scenes.Game)
        {
            int index = UnityEngine.Random.Range(0, inGameAudioClipList.Count - 1);
            this.Log("index: " + index);
            this.Log("inGameAudioClipList.Count: " + inGameAudioClipList.Count);
            return inGameAudioClipList[index];
        }
        else
        {
            // Defualts to gte loading music
            return loadingAudioClipList[UnityEngine.Random.Range(0, loadingAudioClipList.Count - 1)];
        }
    }

    private async void WiatOntillTheMuscHasStoped()
    {
        AudioClip lastAudioClip = currentAudioClip;
        int addedTimeInMilliseconds = 1000;
        await System.Threading.Tasks.Task.Delay(Mathf.RoundToInt((currentAudioClip.length * 1000) + addedTimeInMilliseconds));

        if (lastAudioClip == currentAudioClip)
        {
            OnAudioClipFinished?.Invoke(this, EventArgs.Empty);
        }
    }

}
