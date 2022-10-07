using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    public int health = 2000;
    public MeshRenderer whiteScreen;
    public Volume v;
    public float vignetteMultiplier = 65f;
    public float vgFadeMultiplier = 10f;
    public LayerMask notPlayerMask;

    private Vignette vg;
    private bool loading;
    private Vector3 lastPosition;
    private AudioSource ringingSound;

    private void Start()
    {
        lastPosition = transform.position;

        v.profile.TryGet(out vg);

        whiteScreen.material.DOFade(0, 3);

        ringingSound = GameObject.Find("Ringing").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if ( Inputs.aButtonPressed )
        {
            Time.timeScale = 0.05f;
            Time.fixedDeltaTime = 0.002f;

            foreach ( AudioSource audio in FindObjectsOfType<AudioSource>() )
            {
                if ( audio.gameObject.layer != 10 )
                {
                    audio.pitch = 0.5f;
                }
            }
        }

        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            foreach ( AudioSource audio in FindObjectsOfType<AudioSource>() )
            {
                audio.pitch = 1;
            }
        }

        if ( health < 1 )
        {
            if ( !loading ) 
            {
                StartCoroutine(LoadMainMenu());
            }

            foreach ( AudioSource audio in FindObjectsOfType<AudioSource>() )
            {
                if ( audio != ringingSound )
                {
                    audio.DOFade(0, 1);
                }
            }
        }

        if ( Physics.CheckSphere(transform.position, 0.1f, notPlayerMask) )
        {
            transform.position = lastPosition;
        }

        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.GetComponent<Damage>() )
        {
            health -= other.GetComponent<Damage>().damage;

            vg.intensity.value = (1.0f / health) * vignetteMultiplier;

            StartCoroutine(FlashRed());
        }
    }

    IEnumerator LoadMainMenu()
    {
        loading = true;

        ringingSound.Play();

        whiteScreen.material.DOFade(255, 5);

        yield return new WaitForSeconds(5.3f);

        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator FlashRed()
    {
        float originalValue = vg.intensity.value;

        vg.intensity.value = 100;

        while ( vg.intensity.value > originalValue )
        {
            vg.intensity.value -= Time.unscaledDeltaTime * vgFadeMultiplier;

            yield return new WaitForSecondsRealtime(0.01f);
        }   
    }
}
