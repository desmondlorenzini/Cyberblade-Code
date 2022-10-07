using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrates : MonoBehaviour
{
    public float maxDelay = 30;
    public float minDelay = 20;
    public int minCrates = 2;
    public int maxCrates = 6;
    public float minDropTime = 0.5f;
    public float maxDropTime = 1.3f;
    public GameObject plane;
    public Transform planeSpawnPoint;
    public Transform planeEndPoint;
    public GameObject crate;
    public float planeSpeed = 3;
    public Ease planeEase = Ease.InCirc;
    public int maxCratesAlive;
    
    private float nextFireTime;
    private List<GameObject> crates = new List<GameObject>();

    void Update()
    {
        if ( Time.time > nextFireTime )
        {
            int cratesAlive = 0;

            foreach ( GameObject c in crates )
            {
                if ( c != null )
                {
                    cratesAlive++;
                }
            }

            if ( cratesAlive > maxCratesAlive )
            {
                nextFireTime = Time.time + Random.Range(minDelay / 2, maxDelay / 2);

                return;
            }

            nextFireTime = Time.time + Random.Range(minDelay, maxDelay);

            List<Transform> planes = new List<Transform>();

            for ( int i = 0; i < Random.Range(minCrates, maxCrates); i++ )
            {
                float x = Random.Range(-planeSpawnPoint.localScale.x / 2, planeSpawnPoint.localScale.x / 2);
                float y = Random.Range(-planeSpawnPoint.localScale.y / 2, planeSpawnPoint.localScale.y / 2);
                float z = Random.Range(-planeSpawnPoint.localScale.z / 2, planeSpawnPoint.localScale.z / 2);

                Vector3 spawnPos = planeSpawnPoint.position + new Vector3(x, y, z);

                Transform planeCopy = Instantiate(plane, spawnPos, Quaternion.identity).transform;

                planeCopy.DOMoveX(planeEndPoint.position.x, planeSpeed).SetEase(planeEase);

                planes.Add(planeCopy);

                Destroy(planeCopy.gameObject, planeSpeed);
            }

            StartCoroutine(DropCrates(planes));
        }
    }

    IEnumerator DropCrates(List<Transform> planes)
    {
        yield return new WaitForSeconds(Random.Range(minDropTime, maxDropTime));

        foreach ( Transform p in planes )
        {
            crates.Add(Instantiate(crate, p.position, Quaternion.Euler(new Vector3(-90, 0, 0))));

            yield return new WaitForSeconds(Random.Range(0.05f, 0.4f));
        }
    }
}
