using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text weaponLevel;
    public Text weaponCost;
    public Text moneyText;
    public int moneyPrefs;

    void Update()
    {
        moneyPrefs = PlayerPrefs.GetInt("money", 0);
        moneyText.text = moneyPrefs.ToString();
        weaponLevel.text = PlayerPrefs.GetInt("weaponLevel", 0).ToString();
        weaponCost.text = "UPGRADE ($" + PlayerPrefs.GetInt("weaponCost", 100).ToString() + ")";
    }

    public void UpgradeWeapon()
    {
        if (PlayerPrefs.GetInt("money", 0) >= PlayerPrefs.GetInt("weaponCost", 100))
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) - PlayerPrefs.GetInt("weaponCost", 100));
            PlayerPrefs.SetInt("weaponLevel", PlayerPrefs.GetInt("weaponLevel", 0) + 1);
            PlayerPrefs.SetInt("weaponCost", (int)Mathf.Round(PlayerPrefs.GetInt("weaponCost", 100) * 1.3f));
            PlayerPrefs.SetInt("weaponDamage", (int)Mathf.Round(PlayerPrefs.GetInt("weaponDamage", 10) * 1.3f));
        }
        else
        {
            Debug.Log("Your money is not enough!");
        }
    }

}
