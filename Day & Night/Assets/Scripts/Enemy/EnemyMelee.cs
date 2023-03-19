using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    
    void Awake() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        string otherTransformName = other.transform.name;

        if(other.tag != "Enemy") {
            if(other.tag == "Player"){
                if(otherTransformName == "BossEnemy") {
                    playerController.TakeDamage(10);
                } else {
                    playerController.TakeDamage(25);
                }
            }
            // Instantiate(_particleCollide,transform.position,Quaternion.identity);
        }
    }
}
