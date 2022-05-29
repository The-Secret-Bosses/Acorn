using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    private void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (!playerHealth.hasBeenHit)
        {
            playerHealth.hasBeenHit = true;
            playerHealth.TakeDamage();
        }
    }
}
