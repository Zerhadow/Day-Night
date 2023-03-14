
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float _projectileSpeed;
    // [SerializeField] GameObject _particleCollide;
    GameObject player;
    
    void Awake(){
        player = GameObject.Find("FirstPersonPlayer");
    }

    void Start(){
        
        Destroy(this.gameObject, 15f);
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y +1,player.transform.position.z));
    }

    // Start is called before the first frame update
    void Update()
    {
        transform.Translate(Vector3.forward*_projectileSpeed*Time.deltaTime);
    }

    void OnTriggerEnter(Collider other){
        if(other.tag != "Enemy"){
            if(other.tag == "Player"){
                //have player take damage
                // player.PlayerController.TakeDamage(20);
            }
            // Instantiate(_particleCollide,transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}