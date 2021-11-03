using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject shopPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("money", 999999999);
        }
    }

    public void SelectDifficulty()
    {
        difficultyPanel.gameObject.SetActive(true);
    }

    public void CloseSelectDifficulty()
    {
        difficultyPanel.SetActive(false);
    }

    public void Shop()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }


    public void Easy()
    {
        GamePrefs.levelSelected = "Easy";
        SceneManager.LoadScene("Game");
    }

    public void Medium()
    {
        GamePrefs.levelSelected = "Medium";
        SceneManager.LoadScene("Game");

    }

    public void Hard()
    {
        GamePrefs.levelSelected = "Hard";
        SceneManager.LoadScene("Game");

    }

}
