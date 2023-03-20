using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float lifeSpan = 0f;
    [SerializeField] float damage = 100f;

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
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
}
