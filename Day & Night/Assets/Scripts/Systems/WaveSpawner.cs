using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour {
    public enum SpawnState {SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave {
        public string name;
        [Header("Enemy prefabs")]
        public Transform[] enemy;
        [Header("Number of enemies per type")]
        public int[] enemies;
        public float rate; //amount of time in between mob spawns
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int highestWave = 0;

    public TMP_Text valueText;
    public int waveCount;
    Slider waveSlider;
    int totalEnemies;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    ItemManager itemManager;

    public bool isDay = true;
    public bool isNight = false;

    PlayerController player;

    // DayNightController dayNightController;

    void Awake() {
        waveCount = nextWave + 1;
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        valueText = GameObject.Find("WaveCountText").GetComponent<TMP_Text>();
        valueText.text = "Wave: " + waveCount.ToString();
        waveSlider = GameObject.Find("WaveProgress").GetComponent<Slider>();
        // dayNightController = GameObject.Find("DayNightController").GetComponent<DayNightController>();
    }

    void Start() {
        if(spawnPoints.Length == 0) {
            Debug.Log("No spawn points referenced");
        }

        // dayNightController.UpdateSkyDay();

        waveCountdown = timeBetweenWaves;
    }

    void Update() {
        if(state == SpawnState.WAITING) {
            //check if enemies are still alive
            if(!EnemyIsAlive() && isDay) {
                Debug.Log("Wave completed");
                WaveCompleted();
            } else {
                return;
            }
        }

        if(waveCountdown <= 0) {
            if(state != SpawnState.SPAWNING) {
                if(isDay) //start spawning waves
                    StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            waveCountdown -=Time.deltaTime;
        }

        GetTotalEnemies();
    }

    void FixedUpdate() {

        waveCount = nextWave + 1;
        waveSlider.value = totalEnemies;

        if(player.playerDied) {
            isNight = true;
            isDay = false;
            despawnAllEnemies();
            nextWave = 0; //reset wave count
            // dayNightController.UpdateSkyNight();
        }
    }

    void WaveCompleted() {
        //begin new round
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1) {
            nextWave = 0;
            Debug.Log("All waves complete! Looping. . .");
        } else {
            nextWave++;
            if(nextWave > highestWave)
                highestWave = nextWave;
            itemManager.setSpawnSet(highestWave);
        }
    }

    bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;

        if(searchCountdown <= 0f && isDay) {
            if(GameObject.FindGameObjectWithTag("Enemy") == null) {
                Debug.Log("No enemies alive");
                return false;
            }

            searchCountdown = 1f;
        }        

        return true;
    }

    IEnumerator SpawnWave(Wave wave) {
        Debug.Log("Spawning Wave: " + wave.name);
        state = SpawnState.SPAWNING;

        valueText.text = "Wave: " + waveCount.ToString();

        // if(nextWave != 0)
            // dayNightController.UpdateSkySkyNextWave();

        if(wave.enemy.Length == wave.enemies.Length) {
            for(int i = 0; i < wave.enemy.Length; i++) {
                for(int j = 0; j < wave.enemies[i]; j++) {
                    SpawnEnemy(wave.enemy[i]);
                    yield return new WaitForSeconds(1/wave.rate);
                }
            }
        } else {
            Debug.Log("Enemy and enemy count arrays are not the same length");
        }

        waveSlider.maxValue = totalEnemies;

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy) {
        Debug.Log("Spawning Enemy: " + _enemy.name);

        //spawn enemy; change from random
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, randomSpawnPoint.position, randomSpawnPoint.rotation);
    }

    void despawnAllEnemies() {
        // Debug.Log("Despawning all enemies");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies) {
            Destroy(enemy);
        }
    }

    void GetTotalEnemies() {
        // Debug.Log("Getting total enemies");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
        // Debug.Log("Total enemies: " + totalEnemies);
    }

    private void OnDrawGizmos() {
        //draw spawn points
        Gizmos.color = Color.red;
        foreach(Transform spawnPoint in spawnPoints) {
            Gizmos.DrawWireSphere(spawnPoint.position, 1f);
        }
    }
}