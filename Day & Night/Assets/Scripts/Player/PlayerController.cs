using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {

    [SerializeField] WaveSpawner waveSpawner;
    [SerializeField] ItemManager itemManager;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] DayNightController dayNightController;

    [SerializeField] AudioClip _playerDmg;
    private AudioSource playerDamage;
    [SerializeField] AudioClip _playerDie;
    private AudioSource playerDeath;

    public float maxHP = 100;
    public float currHP = 100;
    public float damage = 10;

    public HUDHealth healthBar;
    GameObject playerHPBar;

    public Animator transition;
    public float transitionTime = 1f;

    [SerializeField] Vector3 daySpawnPt = new Vector3(0,0.59f,0);
    [SerializeField] Vector3 nightSpawnPt = new Vector3(0,0.59f,-25);

    public bool playerDied = false;

    Movement movement;
    public bool inCell = false;

    AudioSource dayTrack;
    AudioSource nightTrack;
    AudioSource cellTrack;
    AudioManager audioManager;

    GameObject zoneAreaObj;
    TMP_Text zoneText;
    Animator zoneAnimator;

    public bool isDay = true;
    public bool isNight = false;

    void Awake() {
        currHP = maxHP;
        playerHPBar = GameObject.Find("PlayerHPBar");
        movement = GetComponent<Movement>();

        StartCoroutine(teleportToDaySpawnCoroutine());

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        dayTrack = GameObject.Find("DayTrack").GetComponent<AudioSource>();
        nightTrack = GameObject.Find("NightTrack").GetComponent<AudioSource>();
        cellTrack = GameObject.Find("CellTrack").GetComponent<AudioSource>();
        // zoneAreaObj = GameObject.Find("ZoneArea");
        // zoneText = GameObject.Find("ZoneText").GetComponent<TMP_Text>();
        // zoneAnimator = GameObject.Find("ZoneText").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        playerDamage = gameObject.AddComponent<AudioSource>();
        playerDamage.clip = _playerDmg;
        playerDeath = gameObject.AddComponent<AudioSource>();
        playerDeath.clip = _playerDie;

        StartCoroutine(AudioManager.StartFade(dayTrack, 3f, 1f));
    }

    // Update is called once per frame
    void Update() {

    }

    public void Heal(float health) {
        if (health > 0) {
            currHP += health;
            currHP = Mathf.Clamp(currHP, 0, maxHP);
        }
    }

    public void TakeDamage(float damage) {
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currHP -= damage;

        playerDamage.Play();
        healthBar.SetHealth(currHP);

        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currHP <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log(transform.name + " fainted.");
        playerDeath.Play();
        isDay = false;
        isNight = true;
        // Fade scene to black, then start night phase
        playerDied = true;
        weaponManager.clear();
        StartCoroutine(FadeOut());
        StartCoroutine(teleportToNightSpawnCoroutine());
        currHP = 1; //player has to go and find health pickups
        //teleport to location to night spawn
        NightPhase();
    }

    IEnumerator FadeOut() {
        transition.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);
    }

    IEnumerator FadeOutText() {
        zoneAnimator.SetTrigger("FadeInText");

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator teleportToNightSpawnCoroutine() {
        Debug.Log("Teleporting to night spawn");
        movement.disabled = true;
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.position = nightSpawnPt;
        yield return new WaitForSeconds(0.1f);
        movement.disabled = false;
    }

    IEnumerator teleportToDaySpawnCoroutine() {
        Debug.Log("Teleporting to day spawn");
        movement.disabled = true;
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.position = daySpawnPt;
        yield return new WaitForSeconds(0.1f);
        movement.disabled = false;
    }

    IEnumerator TransitionToNight() {
        dayNightController.UpdateSkyNight();
        itemManager.spawn();
        StartCoroutine(AudioManager.StartFade(dayTrack, 3f, 0f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(AudioManager.StartFade(nightTrack, 3f, 1f));
    }

    IEnumerator TransitionToDay() {
        dayNightController.UpdateSkyDay();
        itemManager.despawn();
        StartCoroutine(AudioManager.StartFade(nightTrack, 3f, 0f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(AudioManager.StartFade(dayTrack, 3f, 1f));
    }

    public void NightPhase() {
        Debug.Log("Night phase");

        StartCoroutine(TransitionToNight());
    }

    public IEnumerator TransitionToAndFromCell(bool inCell) {
        if(inCell) { // enters cell
            StartCoroutine(AudioManager.StartFade(nightTrack, 3f, 0f));
            yield return new WaitForSeconds(3f);
            StartCoroutine(AudioManager.StartFade(cellTrack, 3f, 1f));
        } else { // exits cell
            StartCoroutine(AudioManager.StartFade(cellTrack, 3f, 0f));
            yield return new WaitForSeconds(3f);
            StartCoroutine(AudioManager.StartFade(nightTrack, 3f, 1f));
        }
    }

    public void DayPhase() {
        Debug.Log("Day phase");
        StartCoroutine(TransitionToDay());
    }

    void OnTriggerEnter(Collider other) {
        string otherTransformName = other.transform.name;

        if (other.tag == "Zone") {
            if(otherTransformName == "CellZone" && isNight) {
                if(!inCell) {
                    Debug.Log("Player has entered zone");
                    inCell = true;
                    StartCoroutine(TransitionToAndFromCell(inCell));
                } else {
                    Debug.Log("Player has left cell");
                    inCell = false;
                    StartCoroutine(TransitionToAndFromCell(inCell));
                }
            } else if(otherTransformName == "TrainingZone") {
                Debug.Log("Player has entered training zone");
                // zoneText.text = "Training Zone";
                StartCoroutine(FadeOutText());
            } else if(otherTransformName == "StagingZone") {
                Debug.Log("Player has entered Staging Yard");
                zoneText.text = "Staging Yard";
            } else if(otherTransformName == "CourtyardZone") {
                Debug.Log("Player has entered Courtyard");
                zoneText.text = "Courtyard";
            } else if(otherTransformName == "StorageZone") {
                Debug.Log("Player has entered storage zone");
                zoneText.text = "Storage Zone";
            } else if(otherTransformName == "TowerZone") {
                Debug.Log("Player has entered tower zone");
                zoneText.text = "Tower Zone";
            } else if(otherTransformName == "MainHallZone") {
                Debug.Log("Player has entered main hall zone");
                zoneText.text = "Main Hall";
            } else if(otherTransformName == "GarrisonZone") {
                Debug.Log("Player has entered garrison zone");
                zoneText.text = "Garrison";
            } else if(otherTransformName == "DungeonZone") {
                Debug.Log("Player has entered dungeon zone");
                zoneText.text = "Dungeon";
            }
        }
    }
}
