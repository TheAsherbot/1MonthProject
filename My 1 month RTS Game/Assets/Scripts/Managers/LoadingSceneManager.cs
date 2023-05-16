using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{


    [SerializeField] private Image loadingProgressImage;


    private bool isFirstUpdate = true;

    
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            SceneLoader.Instance.LoadFromLoadingScene();
        }
        loadingProgressImage.fillAmount = SceneLoader.Instance.GetLoadingPregress();
    }

}
