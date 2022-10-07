using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform player;
    public float speed = 6;
    public float trackDistance = 4;
    public Explosive explosive;
    public List<GameObject> lods = new List<GameObject>();

    private Rigidbody rb;
    private bool exploded;
    private bool reachedDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if ( exploded )
        {
            return;
        }

        if ( Vector3.Distance(transform.position, player.position) < trackDistance )
        {
            reachedDistance = true;
        }

        if ( !reachedDistance )
        {
            transform.LookAt(player);
        }
    }

    private void FixedUpdate()
    {
        if ( exploded )
        {
            return;
        }

        if ( !reachedDistance )
        {
            rb.velocity = transform.forward * speed * Time.deltaTime; 
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ( !collision.gameObject.CompareTag("Don't Collide") )
        {
            Destroy(transform.Find("Smoke").gameObject);

            explosive.Explode();

            exploded = true;

            Destroy(GetComponent<LODGroup>());        
            
            foreach ( GameObject lod in lods )
            {
                Destroy(lod);
            }

            Destroy(gameObject, 5);
        }
    }
}
