using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TheAshBot;

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


    private float timerMaxOffset = 3;
    private float timePastedScenceMusicHasStarted;


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
    }

    private void Update()
    {
        CountTimePastedScenceMusicHasStarted();
    }

    private void AudioManager_OnAudioClipFinished(object sender, EventArgs eventArgs)
    {
        GertAndPlayAudioCLip();
    }

    private void SceneLoader_OnSceneLoaded(object sender, SceneLoader.OnSceneLoadedEventArgs eventArgs)
    {
        GertAndPlayAudioCLip();
    }

    private void GertAndPlayAudioCLip()
    {
        timePastedScenceMusicHasStarted = 0;
        currentAudioClip = GetAudioClipForCurrentScene();
        audioSource.clip = currentAudioClip;
        audioSource.Play();
    }

    private AudioClip GetAudioClipForCurrentScene()
    {
        if (SceneLoader.Instance.scene == SceneLoader.Scenes.MainMenu)
        {
            return mainMenuAudioClipList[UnityEngine.Random.Range(0, mainMenuAudioClipList.Count)];
        }
        else if (SceneLoader.Instance.scene == SceneLoader.Scenes.Loading)
        {
            return loadingAudioClipList[UnityEngine.Random.Range(0, loadingAudioClipList.Count)];
        }
        else if (SceneLoader.Instance.scene == SceneLoader.Scenes.Game)
        {
            int index = UnityEngine.Random.Range(0, inGameAudioClipList.Count);
            return inGameAudioClipList[index];
        }
        else
        {
            // Defualts to gte loading music
            return loadingAudioClipList[UnityEngine.Random.Range(0, loadingAudioClipList.Count)];
        }
    }

    private void CountTimePastedScenceMusicHasStarted()
    {
        timePastedScenceMusicHasStarted += Time.unscaledDeltaTime;
        if (timePastedScenceMusicHasStarted >= currentAudioClip.length + timerMaxOffset)
        {
            timePastedScenceMusicHasStarted = 0;
            OnAudioClipFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
