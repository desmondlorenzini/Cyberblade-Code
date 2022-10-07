using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FirstBoss : MonoBehaviour
{
    public float maxPlayerDist = 4.0f;
    public float maxMoveDelay = 5.0f;
    public float minMoveDelay = 1.0f;
    public float maxTweenTime = 6.0f;
    public float minTweenTime = 1.0f;
    public Transform gunTip;
    public GameObject laser;
    public float laserSpeed = 2f;
    public int laserDamage = 100;
    public float fireRate = 3f;
    public AudioClip laserNoise;
    public float rotTime = 3;

    private float nextMoveTime;
    private float moveTime;
    private float nextRotTime;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(Fire());
    }

    private void Update()
    {
        if (Time.time > nextRotTime)
        {
            nextRotTime = Time.time + rotTime;
            transform.DORotate(transform.eulerAngles + new Vector3(0, 180, 0), rotTime);
        }

        if ( Time.time > nextMoveTime )
        {   
            moveTime = Random.Range(minTweenTime, maxTweenTime);
            nextMoveTime = Time.time + moveTime + Random.Range(minMoveDelay, maxMoveDelay);

            Vector3 pos = player.position;
            pos.y = 0;

            transform.DOMove(pos, moveTime).SetEase(Ease.InOutQuad);
        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            GameObject laserCopy = Instantiate(laser, gunTip.transform.position, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y)));

            laserCopy.transform.DOMove(transform.forward * 100, laserSpeed);
            laserCopy.transform.DOScale(0, laserSpeed / 3);    

            Destroy(laserCopy, 3);

            laserCopy.AddComponent<Damage>().damage = laserDamage;

            SoundManager.PlaySound(laserNoise, 0.5f, true, "SFX");

            yield return new WaitForSeconds(fireRate);
        }
    }
}
