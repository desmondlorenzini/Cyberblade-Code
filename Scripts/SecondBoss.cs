using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : MonoBehaviour
{
    public List<GameObject> legs = new List<GameObject>();
    public float moveTime = 2.1f;
    public float moveRate = 5;
    public GameObject radius;
    public ParticleSystem explosionParticles;
    public AudioClip explosionSound;
    public GameObject dust;
    public AudioClip landSound;

    private Animator anim;
    private float nextMoveTime;
    private Transform player;

    private void Start()
    {
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        transform.position = new Vector3(transform.position.x, -14.5f, transform.position.z);
    }

    private void Update()
    {
        int destroyedLegs = 0;

        foreach ( GameObject leg in legs )
        {
            if ( leg == null )
            {
                destroyedLegs++;
            }
        }

        if ( destroyedLegs > 3 )
        {
            StartCoroutine(Explode());
        }

        if ( Time.time > nextMoveTime )
        {
            nextMoveTime = moveTime + Time.time + moveRate;

            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        anim.Play("Move");

        yield return new WaitForSeconds(1.1f);

        transform.DOMoveY(10, moveTime / 3);
        
        yield return new WaitForSeconds(0.3f);
        
        transform.DOKill();
        transform.DOMoveX(player.position.x, moveTime);
        transform.DOMoveZ(player.position.z, moveTime);

        transform.DOMoveY(-14.7f, moveTime / 1.3f).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(moveTime * 0.77f);

        Destroy(Instantiate(dust, radius.transform.position, dust.transform.rotation), 6);

        SoundManager.PlaySound(landSound, 1, true, "SFX");   

        foreach ( GameObject leg in legs )
        {
            if ( leg == null )
            {
                continue;
            }

            leg.GetComponentInChildren<ParticleSystem>().Play();
        }

        radius.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        radius.SetActive(false);
    }

    IEnumerator Explode()
    {
        gameObject.AddComponent<Rigidbody>();

        yield return new WaitForSeconds(1);

        explosionParticles.transform.parent = null;
            
        Destroy(explosionParticles.gameObject, 5);

        explosionParticles.Play();

        SoundManager.PlaySound(explosionSound, 0.6f, true, "SFX");

        Destroy(gameObject);
    }
}
