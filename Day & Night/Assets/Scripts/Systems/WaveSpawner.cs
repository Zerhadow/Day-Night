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
    public TMP_Text enemiesLeftText;
    public int waveCount;
    int totalEnemies;
    GameObject waveIndicator;
    GameObject waveInfo;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    ItemManager itemManager;

    public bool isDay = true;
    public bool isNight = false;

    PlayerController player;

    GameObject directionalLight;
    DayNightController dayNightController;

    void Awake() {
        waveCount = nextWave + 1;
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        valueText = GameObject.Find("WaveCountText").GetComponent<TMP_Text>();
        enemiesLeftText = GameObject.Find("EnemiesLeftText").GetComponent<TMP_Text>();
        valueText.text = "Wave: " + waveCount.ToString();
        waveIndicator = GameObject.Find("WaveIndicator"); 
        directionalLight = GameObject.Find("Directional Light");
        dayNightController = directionalLight.GetComponent<DayNightController>();
        waveInfo = GameObject.Find("WaveCount");
    }

    void Start() {
        if(spawnPoints.Length == 0) {
            Debug.Log("No spawn points referenced");
        }

        dayNightController.UpdateSkyDay();

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
        // Debug.Log("Enemies left: " + totalEnemies);
        enemiesLeftText.text = "Enemies left: " + totalEnemies.ToString();

        if(player.playerDied) {
            isNight = true;
            isDay = false;
            waveInfo.SetActive(false);
            despawnAllEnemies();
            dayNightController.UpdateSkyNight();
        } else {
            isNight = false;
            isDay = true;
            waveInfo.SetActive(true);
            dayNightController.UpdateSkyDay();
            nextWave = 0; //reset wave count
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

        waveIndicator.SetActive(true);

        valueText.text = "Wave: " + waveCount.ToString();

        if(nextWave != 0)
            dayNightController.UpdateSkyNextWave();

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

        enemiesLeftText.text = "Enemies Left: " + totalEnemies.ToString();

        state = SpawnState.WAITING;
        waveIndicator.SetActive(false);

        yield break;
    }

    void SpawnEnemy(Transform _enemy) {
        // Debug.Log("Spawning Enemy: " + _enemy.name);

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
    }

    private void OnDrawGizmos() {
        //draw spawn points
        Gizmos.color = Color.red;
        foreach(Transform spawnPoint in spawnPoints) {
            Gizmos.DrawWireSphere(spawnPoint.position, 1f);
        }
    }
}