using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject shopPanel;
    public Text moneyText;
    public int moneyPrefs;

    void Start()
    {
        moneyPrefs = PlayerPrefs.GetInt("money", 0);
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
        moneyText.text = moneyPrefs.ToString();
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
