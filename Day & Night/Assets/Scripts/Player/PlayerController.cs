using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxHP = 100;
    public float currHP = 100;
    public float damage = 10;

    public HUDHealth healthBar;

    public Animator transition;
    public float transitionTime = 1f;

    Vector3 daySpawnPt = new Vector3(0,0.59f,0);
    Vector3 nightSpawnPt = new Vector3(0,0.59f,-25);

    public bool playerDied = false;

    Movement movement;
    PotionSpawn potionSpawner;
    public bool inCell = false;

    AudioSource dayTrack;
    AudioSource nightTrack;
    AudioSource cellTrack;
    AudioManager audioManager;

    void Awake() {
        currHP = maxHP;
        movement = GetComponent<Movement>();
        StartCoroutine(teleportToDaySpawnCoroutine());
        potionSpawner = GameObject.Find("Potion Spawner").GetComponent<PotionSpawn>();

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        dayTrack = GameObject.Find("DayTrack").GetComponent<AudioSource>();
        nightTrack = GameObject.Find("NightTrack").GetComponent<AudioSource>();
        cellTrack = GameObject.Find("CellTrack").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
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

        healthBar.SetHealth(currHP);

        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currHP <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log(transform.name + " fainted.");
        // Fade scene to black, then start night phase
        playerDied = true;
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
        StartCoroutine(AudioManager.StartFade(dayTrack, 3f, 0f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(AudioManager.StartFade(nightTrack, 3f, 1f));
    }

    IEnumerator TransitionToDay() {
        StartCoroutine(AudioManager.StartFade(nightTrack, 3f, 0f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(AudioManager.StartFade(dayTrack, 3f, 1f));
    }

    public void NightPhase() {
        Debug.Log("Night phase");
        potionSpawner.SpawnPotion();
        StartCoroutine(TransitionToNight());
    }

    public void DayPhase() {
        Debug.Log("Day phase");
        // potionSpawner.DestroyPotion();
        StartCoroutine(TransitionToDay());
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

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Zone") {
            if(!inCell) {
                Debug.Log("Player has entered zone");
                inCell = true;
                StartCoroutine(TransitionToAndFromCell(inCell));
            } else {
                Debug.Log("Player has left cell");
                inCell = false;
                StartCoroutine(TransitionToAndFromCell(inCell));
            }
        }
    }
}
