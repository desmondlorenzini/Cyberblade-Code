using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public float openTime = 6;
    public Vector3 openPos;
    public Transform door;
    public AudioSource doorSound;
    
    private Vector3 closePos;

    private void Start()
    {
        closePos = door.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( !other.CompareTag("Player") )
        {
            return;
        }

        door.DOLocalMove(openPos, openTime).SetEase(Ease.InSine);

        doorSound.Stop();
        doorSound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if ( !other.CompareTag("Player") )
        {
            return;
        }

        door.DOLocalMove(closePos, openTime).SetEase(Ease.InSine);

        doorSound.Stop();
        doorSound.Play();
    }
}
