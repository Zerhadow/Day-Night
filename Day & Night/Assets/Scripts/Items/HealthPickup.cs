using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float healValue = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject obj = other.gameObject;
            obj.GetComponent<PlayerController>().Heal(healValue);
            Destroy(gameObject);
        }
    }
}
