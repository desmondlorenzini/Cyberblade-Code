using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public float fireRate = 0.06f;
    public int bulletDamage = 3;
    public GameObject bullet;
    public List<Transform> gunTips = new List<Transform>();
    public List<GameObject> flashes = new List<GameObject>();
    public List<AudioClip> shootSounds = new List<AudioClip>();
    public float shootVolume = 1;
    public float attackDist;

    private float nextFireTime;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if ( Vector3.Distance(transform.position, player.transform.position) > attackDist )
        {
            return;
        }

        if ( Time.time > nextFireTime )
        {
            nextFireTime = Time.time + fireRate;

            int index = Random.Range(0, gunTips.Count);

            GameObject bCopy = Instantiate(bullet, gunTips[index].position, Quaternion.Euler(new Vector3(-90, transform.eulerAngles.y, 0)));

            bCopy.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
            
            Destroy(bCopy, 3);

            bCopy.AddComponent<Damage>().damage = bulletDamage;

            SoundManager.PlaySound(shootSounds[Random.Range(0, shootSounds.Count)], shootVolume, true, "SFX");

            flashes[index].SetActive(false);
        }
    }
}
