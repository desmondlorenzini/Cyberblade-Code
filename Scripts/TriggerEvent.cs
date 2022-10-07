using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent e;

    private void OnTriggerEnter(Collider other)
    {
        e.Invoke();
    }

    public void PlayRingingSound()
    {
        AudioSource ringingSound = GameObject.Find("Ringing").GetComponent<AudioSource>();

        ringingSound.Play();
    }
}
