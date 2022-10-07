using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public float musicStopTolerance = 10;
    public bool playerInteracted;
    public AudioSource music;
    public float intenseVolume = 0.35f;
    public float mildVolume = 0.13f;
    public float intenseFadeIn = 0.2f;
    public float mildFadeOut = 2;

    private float timer = 7;

    private void Update()
    {
        timer -= Time.unscaledDeltaTime;
        
        if ( playerInteracted )
        {
            playerInteracted = false;

            timer = 10;

            music.DOKill();
            music.DOFade(intenseVolume, intenseFadeIn);
        }

        else if ( timer <= 0 )
        {   
            music.DOKill();  
            music.DOFade(mildVolume, mildFadeOut);
        }
    }
}
