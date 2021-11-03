using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text waveText;
    private int waveCount;
    public int[] zombieWaveCount;
    public Transform[] zombieSpawnPoint;
    public GameObject zombiePrefabs;
    public Transform zombieParent;
    [SerializeField]
    public static int zombieCountLeft;
    public int zombieCount;
    private bool isSpawning;

    void Start()
    {
        waveCount = 0;
    }

    void Update()
    {
        zombieCount = zombieCountLeft;
        waveText.text = "WAVE " + waveCount.ToString();
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(UpdateWave());
        }

        if (zombieCountLeft <= 0 && isSpawning == false)
        {
            isSpawning = true;
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
            zombieCountLeft++;
            yield return new WaitForSeconds(.2f);
            if (zombieCountLeft >= zombieWaveCount[waveCount - 1])
            {
                Debug.Log("Stop spawn zombies in this wave");
                isSpawning = false;
                StopCoroutine(SpawnZombie());
            }
        }
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
