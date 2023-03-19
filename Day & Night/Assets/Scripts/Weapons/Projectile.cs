using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float lifeSpan = 0f;
    [SerializeField] float velocity = 0f;
    [SerializeField] bool destroyOnImpact = true;

    EnemyController enemy;

    void Awake() {
        enemy = GetComponent<EnemyController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(killSelf());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward);
    }

    IEnumerator killSelf()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Hit player");
            other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy.damage);
        }
    }
}
