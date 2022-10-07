using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MissileRobot : MonoBehaviour
{
    public float maxPlayerDist = 4.0f;
    public float minHeight = -1.0f;
    public float maxHeight = 1.0f;
    public float maxMoveDelay = 5.0f;
    public float minMoveDelay = 1.0f;
    public float maxTweenTime = 6.0f;
    public float minTweenTime = 1.0f;
    public List<Transform> gunTips;
    public List<ParticleSystem> flashes;
    public GameObject rocket;
    public float fireRate = 1.5f;
    public AudioClip rocketNoise;

    private float nextMoveTime;
    private float moveTime;
    private bool canFire;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y + 90, transform.eulerAngles.z);

        if ( Time.time > nextMoveTime )
        {   
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
            int index = Random.Range(0, gunTips.Count);

            Transform tip = gunTips[index];
            flashes[index].Play();

            GameObject missile = Instantiate(rocket, tip.transform.position, Quaternion.identity);

            missile.GetComponent<Missile>().player = player;
            
            Destroy(missile, 30);

            SoundManager.PlaySound(rocketNoise, 1, true, "SFX");

            yield return new WaitForSeconds(fireRate);
        }
    }
}
