using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.Scenes.Game);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
