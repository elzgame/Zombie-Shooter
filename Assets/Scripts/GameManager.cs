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
    private bool isLastWave;
    private bool isWin;
    private DifficultyPrefs difficultyPrefs;
    public static AudioSource audioSource;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public int nextWaveCountdown;
    public GameObject healthKitPrefab;
    public GameObject ammoBoxPrefab;
    public GameObject pausePanel;
    public AudioClip soundClick;
    public AudioClip soundDeath;
    public AudioClip soundWin;

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

        if (waveCount >= zombieWaveCount.Length)
            isLastWave = true;

        if (isLastWave)
        {
            Debug.Log("FINISHH");
            waveCountdownText.text = "LAST WAVE";
        }
        
        if (!isLastWave)
        {
            waveCountdownText.text = "NEXT WAVE IN " + waveCountdownInt.ToString() + "S";
        }

        zombieCount = zombieCountLeft;
        waveText.text = "WAVE " + waveCount.ToString();
        if (zombieCountLeft <= 0 && isSpawning == false || waveCountdown <= 0 && isSpawning == false)
        {
            if (waveCount >= zombieWaveCount.Length)
            {
                isSpawning = true;
                isWin = true;
                waveCountdown = nextWaveCountdown;
                StartCoroutine(UpdateWave());
            }
            else
            {
                isSpawning = true;
                waveCountdown = nextWaveCountdown;
                StartCoroutine(UpdateWave());
            }
        }
        else if (isLastWave && zombieCountLeft <= 0 && isWin)
        {
            isWin = false;
            Win();
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

    public void Win()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
        GameManager.audioSource.PlayOneShot(soundWin);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + PlayerStats.playerMoney);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        GameManager.audioSource.PlayOneShot(soundDeath);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + PlayerStats.playerMoney);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        GameManager.audioSource.PlayOneShot(soundClick);
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.audioSource.PlayOneShot(soundClick);
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GameManager.audioSource.PlayOneShot(soundClick);
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        GameManager.audioSource.PlayOneShot(soundClick);
        pausePanel.SetActive(false);
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
