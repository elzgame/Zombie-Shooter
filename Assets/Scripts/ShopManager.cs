using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text weaponLevel;
    public Text weaponCost;
    public Text knifeLevel;
    public Text knifeCost;
    public Text moneyText;
    public int moneyPrefs;
    public AudioClip soundBuy;
    public AudioClip soundBuyError;


    void Update()
    {
        moneyPrefs = PlayerPrefs.GetInt("money", 0);
        moneyText.text = moneyPrefs.ToString();
        weaponLevel.text = PlayerPrefs.GetInt("weaponLevel", 0).ToString();
        weaponCost.text = "UPGRADE ($" + PlayerPrefs.GetInt("weaponCost", 100).ToString() + ")";
        knifeLevel.text = PlayerPrefs.GetInt("knifeLevel", 0).ToString();
        knifeCost.text = "UPGRADE ($" + PlayerPrefs.GetInt("knifeCost", 50).ToString() + ")";
    }

    public void UpgradeWeapon()
    {
        if (PlayerPrefs.GetInt("money", 0) >= PlayerPrefs.GetInt("weaponCost", 100))
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - PlayerPrefs.GetInt("weaponCost", 100));
            PlayerPrefs.SetInt("weaponLevel", PlayerPrefs.GetInt("weaponLevel", 0) + 1);
            PlayerPrefs.SetInt("weaponCost", (int)Mathf.Round(PlayerPrefs.GetInt("weaponCost", 100) * 1.3f));
            PlayerPrefs.SetInt("weaponDamage", (int)Mathf.Round(PlayerPrefs.GetInt("weaponDamage", 10) * 1.3f));
            MainMenu.audioSource.PlayOneShot(soundBuy);
        }
        else
        {
            MainMenu.audioSource.PlayOneShot(soundBuyError);
            Debug.Log("Your money is not enough!");
        }
    }

    public void UpgradeKnife()
    {
        if (PlayerPrefs.GetInt("money", 0) >= PlayerPrefs.GetInt("knifeCost", 100))
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - PlayerPrefs.GetInt("knifeCost", 100));
            PlayerPrefs.SetInt("knifeLevel", PlayerPrefs.GetInt("knifeLevel", 0) + 1);
            PlayerPrefs.SetInt("knifeCost", (int)Mathf.Round(PlayerPrefs.GetInt("knifeCost", 100) * 1.3f));
            PlayerPrefs.SetInt("knifeDamage", (int)Mathf.Round(PlayerPrefs.GetInt("knifeDamage", 10) * 1.3f));
            MainMenu.audioSource.PlayOneShot(soundBuy);
        }
        else
        {
            MainMenu.audioSource.PlayOneShot(soundBuyError);
            Debug.Log("Your money is not enough!");
        }
    }

}
