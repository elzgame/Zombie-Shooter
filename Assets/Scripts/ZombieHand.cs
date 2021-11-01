using UnityEngine;

public class ZombieHand : MonoBehaviour
{

    public bool isHit;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            isHit = true;
        }
    }
}
