using UnityEngine;

public class GamePrefs : MonoBehaviour
{
    private static GamePrefs menuInstance;
    public static string levelSelected;

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

}
