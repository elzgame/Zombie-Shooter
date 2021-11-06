using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text waveText;
    private int waveCount;
    private float waveCountdown;
    public Text waveCountdownText;
    public int[] zombieWaveCount;
    public Transform[] zombieSpawnPoint;
    public Transform[] healthKitSpawnPoint;
    public Transform[] ammoBoxSpawnPoint;
    public GameObject zombiePrefabs;
    public Transform zombieParent;
    public Transform healthKitParent;
    public Transform ammoBoxParent;
    [SerializeField]
    public static int zombieCountLeft;
    public int zombieCount;
    private bool isSpawning;
    private DifficultyPrefs difficultyPrefs;
    public static AudioSource audioSource;
    public GameObject gameOverPanel;
    public int nextWaveCountdown;
    public GameObject healthKitPrefab;
    public GameObject ammoBoxPrefab;

    void Start()
    {
        isSpawning = false;
        zombieCountLeft = 0;
        waveCountdown = 0;
        audioSource = GetComponent<AudioSource>();
        waveCount = 0;
        difficultyPrefs = FindObjectOfType<DifficultyPrefs>();
        if (GamePrefs.levelSelected == "Easy")
        {
            zombieWaveCount = difficultyPrefs.easyZombieWaveCount;
        }
        else if (GamePrefs.levelSelected == "Medium")
        {
            zombieWaveCount = difficultyPrefs.mediumZombieWaveCount;
        }
        else if (GamePrefs.levelSelected == "Hard")
        {
            zombieWaveCount = difficultyPrefs.hardZombieWaveCount;
        }
        StartCoroutine(SpawnHealthKit());
        StartCoroutine(SpawnAmmoBox());
    }

    void Update()
    {
        waveCountdown -= Time.deltaTime;
        int waveCountdownInt = (int)waveCountdown;
        if (waveCountdown <= 0) waveCountdown = 0;
        waveCountdownText.text = "NEXT WAVE IN " + waveCountdownInt.ToString() + "S";
        zombieCount = zombieCountLeft;
        waveText.text = "WAVE " + waveCount.ToString();
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerStats.playerHealth = 0;
        }

        if (zombieCountLeft <= 0 && isSpawning == false || waveCountdown <= 0 && isSpawning == false)
        {
            isSpawning = true;
            waveCountdown = nextWaveCountdown;
            StartCoroutine(UpdateWave());
        }

    }

    IEnumerator SpawnZombie()
    {
        zombieCountLeft = 0;
        while (true && zombieCountLeft < zombieWaveCount[waveCount - 1])
        {
            var spawnPoint = zombieSpawnPoint[Random.Range(0, zombieSpawnPoint.Length)];
            var zombie = Instantiate(zombiePrefabs, spawnPoint.position, Quaternion.identity);
            zombie.transform.SetParent(zombieParent);
            if (GamePrefs.levelSelected == "Easy")
            {
                zombie.GetComponent<Zombie>().zombieDamage = difficultyPrefs.easyZombieDamage;
                zombie.GetComponent<Zombie>().zombieHealth = difficultyPrefs.easyZombieHealth;
                zombie.GetComponent<Zombie>().zombieSpeed = difficultyPrefs.easyZombieSpeed;
                zombie.GetComponent<Zombie>().zombieMoney = difficultyPrefs.easyZombieMoney;
            }
            else if (GamePrefs.levelSelected == "Medium")
            {
                zombie.GetComponent<Zombie>().zombieDamage = difficultyPrefs.mediumZombieDamage;
                zombie.GetComponent<Zombie>().zombieHealth = difficultyPrefs.mediumZombieHealth;
                zombie.GetComponent<Zombie>().zombieSpeed = difficultyPrefs.mediumZombieSpeed;
                zombie.GetComponent<Zombie>().zombieMoney = difficultyPrefs.mediumZombieMoney;
            }
            else if (GamePrefs.levelSelected == "Hard")
            {
                zombie.GetComponent<Zombie>().zombieDamage = difficultyPrefs.hardZombieDamage;
                zombie.GetComponent<Zombie>().zombieHealth = difficultyPrefs.hardZombieHealth;
                zombie.GetComponent<Zombie>().zombieSpeed = difficultyPrefs.hardZombieSpeed;
                zombie.GetComponent<Zombie>().zombieMoney = difficultyPrefs.hardZombieMoney;
            }
            zombieCountLeft++;
            yield return new WaitForSeconds(.2f);
            if (zombieCountLeft >= zombieWaveCount[waveCount - 1])
            {
                isSpawning = false;
                waveCountdown = nextWaveCountdown;
                StopCoroutine(SpawnZombie());
            }
        }
    }


    IEnumerator SpawnHealthKit()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20, 60));
            var spawnPoint = healthKitSpawnPoint[Random.Range(0, healthKitSpawnPoint.Length)];
            var healthKit = Instantiate(healthKitPrefab, spawnPoint.position, Quaternion.identity);
            healthKit.transform.SetParent(healthKitParent);
        }
    }

    IEnumerator SpawnAmmoBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 60));
            var spawnPoint = ammoBoxSpawnPoint[Random.Range(0, ammoBoxSpawnPoint.Length)];
            var ammoBox = Instantiate(ammoBoxPrefab, spawnPoint.position, Quaternion.identity);
            ammoBox.transform.SetParent(ammoBoxParent);
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + PlayerStats.playerMoney);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator UpdateWave()
    {
        waveCount++;
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        waveText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(SpawnZombie());
    }
}
