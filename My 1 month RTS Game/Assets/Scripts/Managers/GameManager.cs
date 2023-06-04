using TheAshBot;

using TMPro;

using Unity.VisualScripting.Antlr3.Runtime;

using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum Teams
{
    PlayerTeam,
    AITeam,
}




public class GameManager : MonoBehaviour
{
    

    public static GameManager Instance
    {
        get;
        private set;
    }




    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverText;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            this.LogError("THERE IS MORE THAN ONE GAME MANAGER INSTANCE IN THIS SCENE! This should NEVER happen!");
            Debug.Log("This is the 1st instance", this);
            Debug.Log("This is the 2nd instance", Instance);
            return;
        }

        Instance = this;
    }


    public void GameOver(Teams lossingTeam)
    {
        Time.timeScale = 0;

        gameOverUI.SetActive(true);

        gameOverText.text = lossingTeam == Teams.AITeam ? "Victory" : "Try to win next time";
    }


    #region Buttons

    public void OnMainMenuButtomClicked()
    {
        Time.timeScale = 1;

        SceneLoader.Instance.LoadScene(SceneLoader.Scenes.MainMenu);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    #endregion


}
