using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] PlayerCamera cam;
    [SerializeField] Transform lookPoint;

    [Header("Initial Weapons")]
    [SerializeField] GameObject startingSword = null;
    [SerializeField] GameObject testBow = null;
    [SerializeField] GameObject testBlunderbuss = null;
    [SerializeField] GameObject testFireball = null;

    [Header("Parameters")]
    [SerializeField] float timeToSwitch = 0.05f;

    [Header("AudioClips")]
    [SerializeField] AudioClip _weaponSwitch;
    private AudioSource weaponSwitch;
    [SerializeField] AudioClip _weaponPickup;
    private AudioSource weaponPickup;

    GameObject[] weapons = { null, null, null };
    GameObject fireBall = null;
    int weaponIndex = -1;

    Coroutine weaponSwitcher = null;
    bool switching = false;

    public enum WeaponType
    {
        Sword = 0,
        Crossbow = 1,
        Blunderbuss = 2,
        Fireball = 3
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitch = gameObject.AddComponent<AudioSource>();
        weaponSwitch.clip = _weaponSwitch;
        weaponPickup = gameObject.AddComponent<AudioSource>();
        weaponPickup.clip = _weaponPickup;

        if (testBlunderbuss != null)
        {
            AddWeapon(testBlunderbuss, WeaponType.Blunderbuss);
        }

        if (testBow != null)
        {
            AddWeapon(testBow, WeaponType.Crossbow);
        }

        if (startingSword != null)
        {
            AddWeapon(startingSword, WeaponType.Sword);
        }

        if (testFireball != null)
            AddWeapon(testFireball, WeaponType.Fireball);
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponIndex > -1)
            switchWeap();
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
        weaponSwitch.Play();

        switching = true;
        weapons[weaponIndex].SetActive(false);
        weaponIndex = nextWeapon;
        yield return new WaitForSeconds(timeToSwitch);
        weapons[weaponIndex].SetActive(true);
        switching = false;
    }

    public void AddWeapon(GameObject weaponPrefab, WeaponType type)
    {
        weaponPickup.Play();

        GameObject weapon = Instantiate(weaponPrefab);
        weapon.transform.SetParent(cam.transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        int typeIndex = (int)type;
        if (typeIndex > -1 && typeIndex < 3)
        {
            if (weapons[typeIndex] != null)
                Destroy(weapons[typeIndex]);

            weapons[typeIndex] = weapon;
            if (weaponIndex != -1)
                weapons[weaponIndex].SetActive(false);

            weapon.SetActive(true);
            weaponIndex = typeIndex;
        }
        else if (typeIndex == 3)
        {
            fireBall = weapon;
            fireBall.SetActive(true);
        }
        else
        {
            Destroy(weapon);
        }
    }

    public void clear()
    {
        weaponIndex = -1;
        foreach(GameObject weapon in weapons)
            Destroy(weapon);

        Destroy(fireBall);
    }
}
