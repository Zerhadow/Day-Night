using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawn : MonoBehaviour
{
    [System.Serializable]
    public class Potion {
        public GameObject potion;
        public int numOfPotions;
        public float healAmount;
        public float spawnRate;
    }

    public Potion potion;

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnPoints.Length == 0)
            Debug.Log("No spawn points referenced");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPotion() {
        StartCoroutine(SpawnPotionEnum());
    }

    IEnumerator SpawnPotionEnum() {
        Debug.Log("Spawning potions");

        for(int i = 0; i < potion.numOfPotions; i++) {
            Spawn();
            yield return new WaitForSeconds(1/potion.spawnRate);
        }
    }

    void Spawn() {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(potion.potion, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    private void OnDrawGizmos() {
        //draw spawn points
        Gizmos.color = Color.blue;
        foreach(Transform spawnPoint in spawnPoints) {
            Gizmos.DrawWireSphere(spawnPoint.position, 1f);
        }
    }
}
