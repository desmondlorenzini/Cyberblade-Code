using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapons : MonoBehaviour
{
    public bool isLeftHand;
    public AudioClip swordUnsheathe;
    public AudioClip swordSheathe;
    public GameObject throwableSword;
    public float swordThrowSpeed = 20f;
    public AudioClip swordThrowSound; 

    private Transform sword;
    private bool usingSword;

    private void Start()
    {
        sword = transform.GetChild(0).Find("Stabber");
    }

    private void Update()
    {
        if ( isLeftHand )
        {
            if ( Inputs.leftTrigger > 0.6 )
            {
                PullOutSword(270);
            }

            else if ( usingSword && Inputs.canUnequipSword )
            {
                SheatheSword(sword);
            }

            if ( Inputs.leftGrip > 0.6 && usingSword && Inputs.leftHasExtra )
            {
                Inputs.leftHasExtra = false;
                ThrowSword();
            }
        }

        else
        {
            if ( Inputs.rightTrigger > 0.6 )
            {
                PullOutSword(270);
            }

            else if ( usingSword && Inputs.canUnequipSword )
            {
                SheatheSword(sword);
            }

            if ( Inputs.rightGrip > 0.6 && usingSword && Inputs.rightHasExtra )
            {
                Inputs.rightHasExtra = false;
                ThrowSword();
            }
        }
    }

    void ThrowSword()
    {  
        sword.localScale = new Vector3(0, sword.localScale.y, sword.localScale.z);
           
        Rigidbody swordRb = Instantiate(throwableSword, sword.transform.position, sword.transform.rotation).GetComponent<Rigidbody>();

        swordRb.AddForce(transform.parent.forward * swordThrowSpeed, ForceMode.Impulse);

        SoundManager.PlaySound(swordThrowSound, 1, true, "SFX");

        Destroy(swordRb.gameObject, 30);
    }

    void SheatheSword(Transform item)
    {
        if ( usingSword )
        {
            SoundManager.PlaySound(swordSheathe, 1, true, "SFX");
        }
        
        usingSword = false;

        item.DOScaleX(0, 0.15f).SetEase(Ease.OutCubic);
    }

    void PullOutSword(float x)
    {
        if ( !usingSword )
        {
            usingSword = true;

            sword.DOScaleX(x, 0.15f).SetEase(Ease.OutCubic);

            SoundManager.PlaySound(swordUnsheathe, 1, true, "SFX");
        }
    }
}
