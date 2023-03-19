using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {   
    public float maxHP = 100;
    public float currHP = 100;
    public float damage = 10;
    public float attackSpeed = 1;
    public float attackRange = 1;
    public float moveSpeed = 1;

    EnemyAIController enemyAI;

    Slider slider;

    void Awake() {
        currHP = maxHP;
        enemyAI = GetComponent<EnemyAIController>();
        slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TakeDamage(float damage) {
        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        currHP -= damage;

        // Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currHP <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log(transform.name + " died.");
        enemyAI.Death();
        Destroy(this.gameObject,1f);
    }
}
