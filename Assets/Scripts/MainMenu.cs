using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject difficultyPanel;
    public string levelSelected;
    private static MainMenu menuInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (menuInstance == null)
        {
            menuInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectDifficulty()
    {
        difficultyPanel.SetActive(true);
    }

    public void CloseSelectDifficulty()
    {
        difficultyPanel.SetActive(false);
    }

    public void Easy()
    {
        levelSelected = "Easy";
        SceneManager.LoadScene("Game");
    }

    public void Medium()
    {
        levelSelected = "Medium";
        SceneManager.LoadScene("Game");

    }

    public void Hard()
    {
        levelSelected = "Hard";
        SceneManager.LoadScene("Game");

    }
}
