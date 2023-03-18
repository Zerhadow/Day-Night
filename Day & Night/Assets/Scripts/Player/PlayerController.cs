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

    void Awake() {
        currHP = maxHP;
        movement = GetComponent<Movement>();
    }

    // Start is called before the first frame update
    void Start() {
        gameObject.transform.position = daySpawnPt;
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
    }

    IEnumerator FadeOut() {
        transition.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);
    }

    IEnumerator teleportToNightSpawnCoroutine() {
        Debug.Log("Teleporting to night spawn");
        movement.disabled = true;
        yield return new WaitForSeconds(1f);
        gameObject.transform.position = nightSpawnPt;
        yield return new WaitForSeconds(1f);
        movement.disabled = false;
    }
}
