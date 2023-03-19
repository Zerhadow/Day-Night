using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponManager.WeaponType weaponType;
    [SerializeField] GameObject weapon;

    WeaponManager weaponManager;
    bool playerInBounds = false;
    KeyCode INTERACT = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(INTERACT) && playerInBounds)
        {
            weaponManager.AddWeapon(weapon, weaponType);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInBounds = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInBounds = false;
        }
    }
}
