using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserEnemy : MonoBehaviour
{
    public float maxPlayerDist = 8.0f;
    public float minHeight = 0.5f;
    public float maxHeight = 1.0f;
    public float maxMoveDelay = 5.0f;
    public float minMoveDelay = 1.0f;
    public float maxTweenTime = 6.0f;
    public float minTweenTime = 1.0f;
    public List<GameObject> lasers;
    public float rotSpeed = 2;
    public AudioSource laserSound;
    public Vector3 rotationOffset = new Vector3(-90, 0, 0);

    private float nextMoveTime;
    private float moveTime;
    private Transform player;
    private bool canFire;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if ( canFire )
        {
            transform.eulerAngles += new Vector3(0, 1, 0) * Time.deltaTime * rotSpeed;
        }

        else
        {
            Vector3 originalRot = transform.eulerAngles;

            transform.LookAt(player);

            transform.eulerAngles += rotationOffset;

            Vector3 rotatedRot = transform.eulerAngles;

            transform.eulerAngles = originalRot;

            transform.DORotate(rotatedRot, 1);
        }

        if ( Time.time > nextMoveTime )
        {   
            foreach ( GameObject laser in lasers )
            {
                laser.SetActive(false);
            }

            canFire = false;

            laserSound.DOFade(0, 1.2f);
        
            moveTime = Random.Range(minTweenTime, maxTweenTime);
            nextMoveTime = Time.time + moveTime + Random.Range(minMoveDelay, maxMoveDelay);

            Vector3 pos = player.position + new Vector3(Random.Range(-maxPlayerDist, -maxPlayerDist), Random.Range(minHeight, maxHeight), Random.Range(-maxPlayerDist, maxPlayerDist));

            transform.DOMove(pos, moveTime).SetEase(Ease.InOutQuad);

            StartCoroutine(StartFiring(moveTime));
        }
    }

    IEnumerator StartFiring(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);

        canFire = true;

        laserSound.Stop();
        laserSound.volume = 1;
        laserSound.Play();

        foreach ( GameObject laser in lasers )
        {
            laser.SetActive(true);
        }
    }
}
