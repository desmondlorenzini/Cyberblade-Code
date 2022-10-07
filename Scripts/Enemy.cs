using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public float maxPlayerDist = 4.0f;
    public float minHeight = -1.0f;
    public float maxHeight = 1.0f;
    public float maxMoveDelay = 5.0f;
    public float minMoveDelay = 1.0f;
    public float maxTweenTime = 6.0f;
    public float minTweenTime = 1.0f;
    public List<Transform> gunTips = new List<Transform>();
    public List<GameObject> flashes = new List<GameObject>();
    public GameObject bullet;
    public float bulletSpeed = 2f;
    public int bulletDamage = 5;
    public float fireRate = 0.1f;
    public Vector3 rotationOffset = Vector3.zero;
    public bool destroyBullet = true;
    public List<AudioClip> shootSounds = new List<AudioClip>();
    public float shootVolume = 1;
    public float bulletRotYOffset;

    private float nextMoveTime;
    private float moveTime;
    private bool canFire;
    private List<Enemy> enemies = new List<Enemy>();
    private Transform player;
    
    private void Start()
    {
        foreach ( GameObject g in GameObject.FindGameObjectsWithTag("Enemy") )
        {
            enemies.Add(g.GetComponent<Enemy>());
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(player);

        transform.eulerAngles += rotationOffset;

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

        for ( int i = 0; i < enemies.Count; i++ )
        {
            int index = Random.Range(0, enemies.Count);

            if ( enemies[index] == null )
            {
                continue;
            }  

            enemies[index].canFire = false;

            if ( Random.value < 0.3 || i > enemies.Count - 3)
            {
                break;
            }
        }

        while ( canFire )
        {
            int index = Random.Range(0, gunTips.Count);

            Transform tip = gunTips[index];
            flashes[index].SetActive(true);

            GameObject bCopy = Instantiate(bullet, tip.position, Quaternion.Euler(new Vector3(-90, transform.eulerAngles.y + bulletRotYOffset, 0)));

            bCopy.GetComponent<Rigidbody>().AddForce(tip.forward * bulletSpeed, ForceMode.VelocityChange);

            Destroy(bCopy, 3);

            bCopy.AddComponent<Damage>().damage = bulletDamage;

            SoundManager.PlaySound(shootSounds[Random.Range(0, shootSounds.Count)], shootVolume, true, "SFX");

            yield return new WaitForSeconds(fireRate);

            flashes[index].SetActive(false);
        }
    }
}
