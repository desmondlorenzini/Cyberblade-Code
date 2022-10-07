using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using DG.Tweening;
using UnityEngine.Animations;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;
    public GameObject rippleEffect;
    public GameObject sparks;
    public AudioClip eyeHitSound;
    public Material brokenGlassMat;
    public float kb = 6f;

    private float speed;
    private Vector3 lastPos;
    private Transform controller;
    private XRNode node;
    private bool isThrowable;
    private Transform tip;
    private Transform fakeParent;
    private Vector3 oldFakeParentPos;
    private Vector3 oldFakeParentRot;

    private void Start()
    {
        lastPos = transform.position;

        isThrowable = transform.CompareTag("Throwable Sword");
        tip = transform.Find("Tip");

        if ( isThrowable )
        {
            return;
        }

        controller = transform.parent.parent.parent.parent;

        node = controller.GetComponent<XRController>().controllerNode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( speed > 2 / Time.timeScale )
        {
            Explosive explosive = other.GetComponent<Explosive>();

            if ( explosive )
            {
                explosive.Explode();
                return;
            }

            if ( other.gameObject.layer == 11 && isThrowable ) // 11 is the stabbable layer
            {
                Stab(other.transform);
            }
        }

        if ( speed < 4 / Time.timeScale && !other.CompareTag("Bullet") )
        {
            return;
        }

        slicer.isTouched = true;
    }

    private void LateUpdate()
    {
        if ( fakeParent != null )
        {
            transform.position = fakeParent.position;
            transform.eulerAngles = fakeParent.eulerAngles;
        }
    }

    private void Update()
    {
        if ( controller == null )
        {
            speed = 100f;

            return;
        }

        if ( node == XRNode.LeftHand )
        {
            rippleEffect.SetActive(Inputs.leftHasExtra);
        }
        
        else
        {
            rippleEffect.SetActive(Inputs.rightHasExtra);
        }

        speed = Vector3.Distance(controller.localPosition, lastPos) / Time.deltaTime;
        
        lastPos = controller.localPosition;

    }

    void Stab(Transform objToStab)
    {
        objToStab.gameObject.layer = 0;

        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<SpawnRipples>());

        GameObject sparkParticles = Instantiate(sparks, tip.position, transform.rotation);
        sparkParticles.transform.eulerAngles += new Vector3(0, 90, 0);

        Destroy(sparkParticles, 6);

        MeshRenderer eye = objToStab.GetComponent<MeshRenderer>();

        if ( objToStab.parent != null )
        {
            objToStab = objToStab.parent;
        }

        GameObject obj = new GameObject();

        obj.name = "Sword Blade Position";
        obj.transform.SetParent(objToStab, true);

        obj.transform.position = tip.position;
        obj.transform.rotation = transform.rotation;

        fakeParent = obj.transform;

        foreach ( Component c in objToStab.GetComponents<Component>() )
        {
            if ( objToStab.GetComponent<MeshRenderer>() == c || objToStab.GetComponent<MeshFilter>() == c || objToStab.GetComponent<Transform>() == c )
            {
                continue;
            }

            if ( c == objToStab.GetComponent<Collider>() ) 
            {
                objToStab.GetComponent<Collider>().enabled = false;

                continue;
            }

            Destroy(c);
        }

        SoundManager.PlaySound(eyeHitSound, 1, true, "SFX");
        
        eye.material = brokenGlassMat;

        objToStab.DOKill();
        objToStab.GetComponent<Collider>().enabled = true;

        objToStab.gameObject.AddComponent<Rigidbody>().AddForce((objToStab.position - transform.position) * kb, ForceMode.Impulse);

        Destroy(objToStab, 30);

        objToStab.tag = "Killed";
    }
}
