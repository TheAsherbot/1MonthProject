using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private AudioManager audioManager;


    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Instantiate(audioManager);
        }
    }

    public void Play()
    {
        SceneLoader.LoadScene(SceneLoader.Scenes.Game);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
