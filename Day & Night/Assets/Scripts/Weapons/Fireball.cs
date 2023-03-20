using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] GameObject explosion = null;
    [SerializeField] float lifeSpan = 5f;
    [SerializeField] float velocity = 20f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(killSelf());
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.magnitude > 750.0f) {
            Destroy(this.gameObject);
        }

        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    IEnumerator killSelf()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {
            Debug.Log("Hit enemy");
            other.GetComponent<EnemyController>().TakeDamage(20);
        }

        if(other.tag != "Player")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
