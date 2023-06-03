using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{


    private static SceneLoader instance;

    public static SceneLoader Instance
    {
        get
        {
            if (instance == null) instance = new SceneLoader();
            return instance;
        }
    }

    private class LoadingMonoBehavior : MonoBehaviour { }


    public enum Scenes
    {
        MainMenu = 0,
        Loading = 1,
        Game = 2,
    }


    public event EventHandler<OnSceneLoadedEventArgs> OnSceneLoaded;
    public class OnSceneLoadedEventArgs : EventArgs
    {
        public Scenes scene;
    }


    public Scenes scene
    {
        get;
        private set;
    } = Scenes.MainMenu;


    private Scenes sceneBeingLoaded;
    private Action OnLoadFromLoadingScene;
    private AsyncOperation loadingAsyncOperation;


    public void LoadScene(Scenes scene)
    {
        // Subscribing to the event
        OnLoadFromLoadingScene += () =>
        {
            LoadSceneAsync(scene);
        };

        // Load the loading scene
        SceneManager.LoadScene(Scenes.Loading.ToString());
        this.scene = Scenes.Loading;
        OnSceneLoaded?.Invoke(this, new OnSceneLoadedEventArgs
        {
            scene = scene,
        });
    }

    private void LoadSceneAsync(Scenes scene)
    {
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        loadingAsyncOperation.completed += LoadingAsyncOperation_completed;
        sceneBeingLoaded = scene;
    }

    private void LoadingAsyncOperation_completed(AsyncOperation obj)
    {
        scene = sceneBeingLoaded;
        OnSceneLoaded?.Invoke(this, new OnSceneLoadedEventArgs
        {
            scene = sceneBeingLoaded,
        });
    }

    public float GetLoadingPregress()
    {
        return loadingAsyncOperation == null ? 1f : loadingAsyncOperation.progress;
    }


    /// <summary>
    /// DO NOT CALL THIS FROM ANY SCRIPT BESIDES "LoadingSceneMnager". Thank you.
    /// </summary>
    public void LoadFromLoadingScene()
    {
        // Envoke the event then get rid of all subscribers
        OnLoadFromLoadingScene?.Invoke();
        OnLoadFromLoadingScene = null;
    }


}
