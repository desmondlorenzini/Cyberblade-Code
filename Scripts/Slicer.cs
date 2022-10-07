using UnityEngine;
using EzySlice;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class Slicer : MonoBehaviour
{
    public Material materialAfterSlice;
    public LayerMask sliceMask;
    public bool isTouched;
    public List<AudioClip> sliceSounds = new List<AudioClip>();
    public float sliceDelay = 0.07f;
    public Material hollowMaterial;
    public float tweenSpeed = 7.5f;

    private float nextSliceTime;
    private MusicManager musicManager;
    private bool isHollow;
    private Transform controller;
    private XRNode node;

    private void Start()
    {
        musicManager = (MusicManager)FindObjectOfType(typeof(MusicManager));

        controller = transform.parent.parent.parent.parent.parent;

        node = controller.GetComponent<XRController>().controllerNode;
    }

    private void Update()
    {
        if ( Time.unscaledTime < nextSliceTime )
        {
            return;
        }

        if (isTouched == true)
        {
            nextSliceTime = Time.unscaledTime + sliceDelay;

            isTouched = false;

            Collider[] objectsToBeSliced = Physics.OverlapBox(transform.position, new Vector3(1, 0.1f, 0.1f), transform.rotation, sliceMask);

            foreach (Collider objectToBeSliced in objectsToBeSliced)
            {                   
                isHollow = objectToBeSliced.CompareTag("Crate");

                Material materialToUse;

                if ( isHollow )
                {
                    Transform extraBlade = objectToBeSliced.transform.Find("Sword");

                    extraBlade.parent = null;

                    StartCoroutine(TweenToPos(extraBlade, true));

                    materialToUse = hollowMaterial;
                }

                else
                {
                    materialToUse = materialAfterSlice;
                }

                SlicedHull slicedObject = SliceObject(objectToBeSliced.gameObject, materialToUse);

                GameObject upperHullGameobject;
                GameObject lowerHullGameobject;

                upperHullGameobject = slicedObject.CreateUpperHull(objectToBeSliced.gameObject, materialToUse);
                lowerHullGameobject = slicedObject.CreateLowerHull(objectToBeSliced.gameObject, materialToUse);

                objectToBeSliced.gameObject.layer = 0;

                upperHullGameobject.transform.position = objectToBeSliced.transform.position;
                lowerHullGameobject.transform.position = objectToBeSliced.transform.position;

                MakeItPhysical(upperHullGameobject); 
                MakeItPhysical(lowerHullGameobject);

                Destroy(objectToBeSliced.gameObject);
            }
        }
    }

    private void MakeItPhysical(GameObject obj)
    {
        if ( musicManager )
        {
            musicManager.playerInteracted = true;
        }

        Inputs.playerInteracted = true;

        obj.AddComponent<MeshCollider>().convex = true;

        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider>(), obj.GetComponent<MeshCollider>());

        obj.AddComponent<Rigidbody>();

        if ( !isHollow )
        {
            obj.layer = 3;
        }

        Destroy(obj, 30);

        if ( sliceSounds.Count > 0 )
        {
            SoundManager.PlaySound(sliceSounds[Random.Range(0, sliceSounds.Count)], 0.3f, true, "SFX");
        }

        isHollow = true;
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        if ( !isHollow )
        {
            return obj.Slice(transform.position, transform.up, crossSectionMaterial);
        }

        else
        {
            return obj.Slice(transform.position, transform.up, hollowMaterial);
        }
    }

    IEnumerator TweenToPos(Transform target, bool destroyWhenDone)
    {
        yield return new WaitForSecondsRealtime(0.1f);

        float rotSpeed = 2.5f;

        while ( true )
        {
            float step = tweenSpeed * Time.deltaTime;

            target.position = Vector3.MoveTowards(target.position, transform.position, step);
            target.rotation = Quaternion.Lerp(target.rotation, transform.rotation, step * rotSpeed);
            
            bool rotIsClose = target.eulerAngles.magnitude < transform.eulerAngles.magnitude + 1f && target.eulerAngles.magnitude > transform.eulerAngles.magnitude - 1f;

            if ( rotIsClose )
            {
                rotSpeed = 3.2f;
            }

            bool rotIsSame = target.eulerAngles.magnitude < transform.eulerAngles.magnitude + 0.1f && target.eulerAngles.magnitude > transform.eulerAngles.magnitude - 0.1f;

            if (Vector3.Distance(transform.position, target.position) < 0.001f && rotIsSame)
            {
                if ( node == XRNode.LeftHand )
                {
                    Inputs.leftHasExtra = true;
                }

                else
                {
                    Inputs.rightHasExtra = true;
                }

                if ( destroyWhenDone )
                {
                    Destroy(target.gameObject);
                }

                break;
            }

            yield return null;
        }
    }
}
