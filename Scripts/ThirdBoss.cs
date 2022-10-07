using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThirdBoss : MonoBehaviour
{
    public float maxPlayerDist = 4.0f;
    public float minHeight = 0.0f;
    public float maxHeight = 2.0f;
    public float maxMoveDelay = 5.0f;
    public float minMoveDelay = 1.0f;
    public float maxTweenTime = 6.0f;
    public float minTweenTime = 1.0f;
    public List<Transform> gunTips = new List<Transform>();
    public List<GameObject> flashes = new List<GameObject>();
    public GameObject bullet;
    public int bulletDamage = 1;
    public float fireRate = 0.1f;
    public GameObject battery;
    public ParticleSystem smoke;
    public List<GameObject> meshes = new List<GameObject>();
    public Explosive explosive;
    public float shootVolume = 0.2f;
    public List<AudioClip> shootSounds;

    private float nextMoveTime;
    private float moveTime;
    private bool canFire;
    private Transform player;
    private bool died;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if ( died )
        {
            return;
        }

        if ( explosive.particlesPlayed )
        {
            explosive.transform.parent = null;

            foreach ( GameObject mesh in meshes )
            {
                Destroy(mesh);
            }

            Destroy(gameObject);

            died = true;
        }

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if ( Time.time > nextMoveTime )
        {   
            foreach ( GameObject flash in flashes )
            {
                flash.SetActive(false);
            }

            moveTime = Random.Range(minTweenTime, maxTweenTime);
            nextMoveTime = Time.time + moveTime + Random.Range(minMoveDelay, maxMoveDelay);

            Vector3 pos = player.position + new Vector3(Random.Range(-maxPlayerDist, -maxPlayerDist), Random.Range(minHeight, maxHeight), Random.Range(-maxPlayerDist, maxPlayerDist));

            transform.DOMove(pos, moveTime).SetEase(Ease.InOutQuad);

            canFire = false;

            StartCoroutine(StartFiring(moveTime));
        }
    }

    IEnumerator StartFiring(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);

        canFire = true;

        while ( canFire )
        {
            List<GameObject> spawnedFlashes = new List<GameObject>();

            int index = Random.Range(0, gunTips.Count);

            Transform tip = gunTips[index];
            flashes[index].SetActive(true);

            spawnedFlashes.Add(flashes[index]);

            GameObject bCopy = Instantiate(bullet, tip.transform.position, Quaternion.Euler(new Vector3(-90, transform.eulerAngles.y, 0)));

            bCopy.transform.localScale = new Vector3(2f, 2f, 2f);

            Destroy(bCopy, 30);

            bCopy.AddComponent<Damage>().damage = bulletDamage;
            HomeToPlayer h = bCopy.AddComponent<HomeToPlayer>();

            h.speed = 500;
            h.trackDistance = 4;

            SoundManager.PlaySound(shootSounds[Random.Range(0, shootSounds.Count)], shootVolume, true, "SFX");

            yield return new WaitForSeconds(fireRate);

            foreach ( GameObject flash in spawnedFlashes )
            {
                flash.SetActive(false);
            }

            spawnedFlashes.Clear();
        }
    }
}
