using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public float force = 0.1f;
    public Vector3 direction = Vector3.up;

    private void OnTriggerStay(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            rb.velocity += new Vector3(0, force * Time.deltaTime, 0);
        }
    }
}
