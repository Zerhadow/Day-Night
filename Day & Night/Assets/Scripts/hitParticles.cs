using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitParticles : MonoBehaviour
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
}
