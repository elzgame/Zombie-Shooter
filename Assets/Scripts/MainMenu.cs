using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject difficultyPanel;
    public GameObject shopPanel;
    public static AudioSource audioSource;
    public AudioClip soundClick;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         PlayerPrefs.DeleteAll();
    //         PlayerPrefs.SetInt("money", 999999999);
    //     }
    // }

    public void SelectDifficulty()
    {
        audioSource.PlayOneShot(soundClick);
        difficultyPanel.gameObject.SetActive(true);
    }

    public void CloseSelectDifficulty()
    {
        audioSource.PlayOneShot(soundClick);
        difficultyPanel.SetActive(false);
    }

    public void Shop()
    {
        audioSource.PlayOneShot(soundClick);
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        audioSource.PlayOneShot(soundClick);
        shopPanel.SetActive(false);
    }


    public void Easy()
    {
        audioSource.PlayOneShot(soundClick);
        GamePrefs.levelSelected = "Easy";
        SceneManager.LoadScene("Game");
    }

    public void Medium()
    {
        audioSource.PlayOneShot(soundClick);
        GamePrefs.levelSelected = "Medium";
        SceneManager.LoadScene("Game");

    }

    public void Hard()
    {
        audioSource.PlayOneShot(soundClick);
        GamePrefs.levelSelected = "Hard";
        SceneManager.LoadScene("Game");

    }

}
