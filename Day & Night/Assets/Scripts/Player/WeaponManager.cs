using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] Camera cam;
    [SerializeField] Transform lookPoint;

    [Header("Initial Weapons")]
    [SerializeField] GameObject startingSword = null;
    [SerializeField] GameObject testBow = null;
    [SerializeField] GameObject testBlunderbuss = null;
    [SerializeField] GameObject testFireball = null;

    [Header("Parameters")]
    [SerializeField] float timeToSwitch = 0.05f;

    GameObject[] weapons = { null, null, null };
    GameObject fireBall = null;
    int weaponIndex = -1;

    Coroutine weaponSwitcher = null;
    bool switching = false;

    public enum WeaponType
    {
        Sword = 0,
        Crossbow = 1,
        Blunderbus = 2,
        Fireball = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startingSword != null) {
            AddWeapon(startingSword, WeaponType.Sword);
            startingSword.SetActive(true);
        }

        if (testBow != null)
        {
            AddWeapon(testBow, WeaponType.Crossbow);
            testBow.SetActive(false);
        }

        if (testBlunderbuss != null)
        {
            AddWeapon(testBlunderbuss, WeaponType.Blunderbus);
            testBlunderbuss.SetActive(false);
        }

        if (testFireball != null)
            AddWeapon(testFireball, WeaponType.Fireball);
    }

    // Update is called once per frame
    void Update()
    {
        align();
        if (weaponIndex > -1)
            switchWeap();
    }

    void align()
    {
        transform.position = lookPoint.position;
        transform.localRotation = cam.GetRotation();
    }

    void switchWeap()
    {
        if(Input.GetKey(KeyCode.Alpha1) && weaponIndex != 0 && weapons[0] != null)
        {
            if (switching)
                StopCoroutine(weaponSwitcher);
            weaponSwitcher = StartCoroutine(SwitchWeapons(0));
        }
        else if(Input.GetKey(KeyCode.Alpha2) && weaponIndex != 1 && weapons[1] != null)
        {
            if (switching)
                StopCoroutine(weaponSwitcher);
            weaponSwitcher = StartCoroutine(SwitchWeapons(1));
        }
        else if(Input.GetKey(KeyCode.Alpha3) && weaponIndex != 2 && weapons[2] != null)
        {
            if (switching)
                StopCoroutine(weaponSwitcher);
            weaponSwitcher = StartCoroutine(SwitchWeapons(2));
        }
    }

    IEnumerator SwitchWeapons(int nextWeapon)
    {
        switching = true;
        weapons[weaponIndex].SetActive(false);
        weaponIndex = nextWeapon;
        yield return timeToSwitch;
        weapons[weaponIndex].SetActive(true);
        switching = false;
    }

    public void AddWeapon(GameObject weapon, WeaponType type)
    {
        int typeIndex = (int)type;
        if (typeIndex > -1 && typeIndex < 4)
        {
            weapons[typeIndex] = weapon;
            weaponIndex = typeIndex;
        }
        else if (typeIndex == 4)
            fireBall = weapon;
            fireBall.SetActive(true);
    }
}
