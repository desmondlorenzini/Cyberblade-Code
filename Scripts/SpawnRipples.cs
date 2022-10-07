using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRipples : MonoBehaviour
{
    public float rippleDelay = 0.35f;
    public GameObject ripple;

    private float nextFireTime;

    private void Update()
    {
        if ( Time.time < nextFireTime )
        {
            return;
        }

        nextFireTime = Time.time + rippleDelay;

        Quaternion rotation = Quaternion.Euler(ripple.transform.eulerAngles + transform.eulerAngles);

        Destroy(Instantiate(ripple, transform.position, rotation), 0.75f);
    }

}
