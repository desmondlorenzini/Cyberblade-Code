using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaveSystem : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> specialEnemies;
    public List<GameObject> bosses;
    public Transform player;
    public GameObject barrel;
    public int wave = 0;
    public float difficultyProgression = 0.25f;
    public float maxDifficulty = 5;
    public int maxSpecialEnemiesToSpawn = 3;
    public float maxBossesToSpawn = 2;

    private List<GameObject> currentEnemies = new List<GameObject>();
    private int spawnAmount = 5;
    private List<GameObject> barrels = new List<GameObject>();
    private int nextBoss = 0;
    private List<int> wavesPassed = new List<int> {0};
    private float difficulty;
    private int specialEnemiesToSpawn;
    private float bossesToSpawn;

    private void Start()
    {
        wave = ES3.Load<int>("Spawn Wave");

        if ( ES3.KeyExists("Waves Passed") )
        {
            wavesPassed = ES3.Load<List<int>>("Waves Passed");
        }
    }
     
    private void Update()
    {  
        int usedBarrels = 0;

        foreach ( GameObject barrel in barrels )
        {
            if ( barrel == null )
            {
                usedBarrels++;
            }
        }

        if ( usedBarrels == barrels.Count )
        {
            for ( int i = 0; i < 20; i++)
            {
                Vector3 rayPos = Vector3.zero + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

                Ray ray = new Ray(rayPos, Vector3.down);

                if ( Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Terrain")) )
                {
                    Vector3 spawnPos = hit.point + new Vector3(0, -18, 0);
                    Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);

                    GameObject b = Instantiate(barrel, spawnPos, rot);

                    barrels.Add(b);

                    b.transform.DOMoveY(hit.point.y, 3);
                }
            }
        }

        int deadEnemies = 0;

        foreach ( GameObject enemy in currentEnemies )
        {
            if ( enemy == null )
            {
                deadEnemies++;
            }

            else if ( enemy.CompareTag("Killed") ) // While most enemies get destroyed when killed, some don't. So we need a second check.
            {
                deadEnemies++;
            }
        }

        if ( deadEnemies < currentEnemies.Count )
        {
            return;
        }

        currentEnemies.Clear();

        wave++;

        if ( !wavesPassed.Contains(wave) )
        {
            wavesPassed.Add(wave);

            ES3.Save("Waves Passed", wavesPassed);
        }

        foreach ( GameObject barrel in barrels )
        {
            Destroy(barrel);   
        }

        if ( wave % 5 == 0 )
        {
            if ( specialEnemiesToSpawn < maxSpecialEnemiesToSpawn )
            {
                specialEnemiesToSpawn++;
            }

            if ( bossesToSpawn < maxBossesToSpawn )
            {
                bossesToSpawn += 0.5f;
            }

            SpawnBoss();

            return;
        }

        bool addedSpecial = false;
        bool addedBoss = false;

        for ( int i = 0; i < spawnAmount; i++ )
        {
            Vector3 spawnPos = Vector3.zero + new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));

            int min = Mathf.RoundToInt(difficulty);
            int max = min + (int)maxDifficulty;

            if ( max > enemies.Count )
            {
                max = enemies.Count;
            }

            currentEnemies.Add(Instantiate(enemies[Random.Range(min, max)], spawnPos, Quaternion.identity));

            if ( Random.value < 0.4 && !addedSpecial )
            {
                for ( int j = 0; j < specialEnemiesToSpawn; j++)
                {
                    currentEnemies.Add(Instantiate(specialEnemies[Random.Range(0, specialEnemies.Count)], spawnPos, Quaternion.identity));
                }
            }

            if ( Random.value < 0.1 && !addedBoss )
            {
                for ( int k = 0; k < bossesToSpawn; k++ )
                {
                    SpawnBoss();
                }
            }

            addedSpecial = true; 
        }

        if ( difficulty < enemies.Count - maxDifficulty )
        {
            difficulty += difficultyProgression;
        }

        spawnAmount++;
    }

    void SpawnBoss()
    {
        nextBoss = (wave / 5) - 1;

        if ( nextBoss >= bosses.Count )
        {
            nextBoss = Random.Range(0, bosses.Count);
        }

        Vector3 spawnPos = Vector3.zero + new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));

        GameObject boss = Instantiate(bosses[nextBoss], spawnPos, bosses[nextBoss].transform.rotation);

        if ( boss.CompareTag("Boss Parent") )
        {
            foreach ( Transform child in boss.transform )
            {
                currentEnemies.Add(child.gameObject);
            }
        }

        else
        {
            currentEnemies.Add(boss);
        }
    }
}
