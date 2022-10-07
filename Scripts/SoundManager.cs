using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static void PlaySound(AudioClip audio, float volume, bool destroy, string tag)
    {
        GameObject obj = new GameObject();

        obj.tag = tag;

        obj.AddComponent<AudioSource>().PlayOneShot(audio, volume);

        if ( destroy )
        {
            Destroy(obj, audio.length);
        }
    }
}
