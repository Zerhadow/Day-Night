using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    [SerializeField] float lifeSpan = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(killSelf());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator killSelf()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy") {
            Debug.Log("Hit enemy");
            other.GetComponent<EnemyController>().TakeDamage(10);
        }
    }
}
