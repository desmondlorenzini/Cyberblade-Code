using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeToPlayer : MonoBehaviour
{
    public float speed = 6;
    public float trackDistance = 4;
    public Explosive explosive;

    private Rigidbody rb;
    private bool reachedDistance;
    private Transform player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if ( Vector3.Distance(transform.position, player.position) < trackDistance )
        {
            reachedDistance = true;
        }

        if ( !reachedDistance )
        {
            transform.LookAt(player);
            transform.eulerAngles += new Vector3(-100, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if ( !reachedDistance )
        {
            rb.velocity = -transform.up * speed * Time.deltaTime; 
        }
    }
}
