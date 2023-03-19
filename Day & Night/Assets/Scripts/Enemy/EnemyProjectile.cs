
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {
    [SerializeField] float _projectileSpeed;
    // [SerializeField] GameObject _particleCollide;
    GameObject player;
    PlayerController playerController;
    public int rangeDamage;
    
    void Awake() {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Start() {
        // Destroy(this.gameObject, 15f);
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y,player.transform.position.z));
    }

    // Start is called before the first frame update
    void Update()
    {
        if(transform.position.magnitude > 750.0f) {
            Destroy(this.gameObject);
        }

        transform.Translate(Vector3.forward*_projectileSpeed*Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag != "Enemy") {
            if(other.tag == "Player"){
                // Debug.Log("Player takes damage");
                playerController.TakeDamage(rangeDamage); //range enemies do 10 damage
            }
            // Instantiate(_particleCollide,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}