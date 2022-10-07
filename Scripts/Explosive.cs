using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public ParticleSystem explosion;
    public float explosionForce = 50;
    public float explosionRadius = 15;
    public float upForce = 500;
    public GameObject radius;
    public List<AudioClip> explosionSounds = new List<AudioClip>();
    public bool addPlayerForce = true;
    public bool particlesPlayed;

    private Rigidbody player;
    private bool hasExploded;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    public void Explode()
    {
        if ( hasExploded )
        {
            return;
        }

        hasExploded = true;

        explosion.Play();

        StartCoroutine(AddExplosionForce());
    }

    IEnumerator AddExplosionForce()
    {
        yield return new WaitForSeconds(0.095f);

        SoundManager.PlaySound(explosionSounds[Random.Range(0, explosionSounds.Count)], 1, true, "SFX");

        if (GetComponent<MeshRenderer>())
        {
            Destroy(GetComponent<MeshRenderer>());
        }

        if (GetComponent<CapsuleCollider>())
        {
            Destroy(GetComponent<CapsuleCollider>());
        }

        Destroy(gameObject, 5);

        radius.SetActive(true);

        Destroy(radius, 0.2f);

        if ( Time.timeScale < 1 )
        {
            explosionForce /= 2;
            explosionRadius /= 2;
            upForce /= 2;
        }

        particlesPlayed = true;

        if ( addPlayerForce )
        {
            player.AddExplosionForce(explosionForce / Time.timeScale, transform.position, explosionRadius / Time.timeScale, upForce / Time.timeScale);
        }
    }
}
