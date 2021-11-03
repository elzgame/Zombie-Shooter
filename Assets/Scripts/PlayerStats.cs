using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int playerHealth;
    public static int playerMoney;
    public Text healthText;
    public Text moneyText;
    private GameManager gameManager;

    void Start()
    {
        playerHealth = 100;
        playerMoney = 0;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        healthText.text = playerHealth.ToString();
        moneyText.text = playerMoney.ToString();
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            gameManager.GameOver();
            Debug.Log("Player die! Game over!");
        }
    }

}
