using UnityEngine;

public class HealthKit : MonoBehaviour
{
    public int heal;
    public AudioClip healSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats.playerHealth += heal;
            GameManager.audioSource.PlayOneShot(healSound);
            Destroy(this.gameObject);
            Debug.Log("Heal");
        }
    }

}
