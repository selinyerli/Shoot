using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public float speed = 2.0f;
    public float damage = 10;
    public float range = 100;
    public Camera fpsCam;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public Rigidbody bullet;
    public Transform spawner;
    public bool isShoot = false;

    public HealthBar healthBar;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Shoot());
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 distance = this.transform.position - player.transform.position;
        distance.y = 0;

        if (Physics.Linecast(this.transform.position, player.transform.position, out hit, -1))
        {
            if (hit.transform.CompareTag("Player"))
            {
                if (distance.magnitude > 5)
                {
                    this.transform.Translate(Vector3.forward * 2f * Time.deltaTime);
                    this.transform.LookAt(player.transform);
                    isShoot = false;
                }
                else
                {
                    this.transform.LookAt(player.transform);
                    isShoot = true;
                }
            }
        }

        agent.destination = player.position;
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (isShoot)
            {
                Rigidbody clone = Instantiate(bullet, spawner.position, Quaternion.identity);
                clone.linearVelocity = spawner.forward * 1000;
            }
        }
    }

    void RayCastForAI()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            AIHealth target = hit.transform.GetComponent<AIHealth>();
            if (target != null)
            {
                target.Takedamage(damage);
            }
            else
            {
                Debug.LogError("AIHealth component is missing on the hit object: " + hit.transform.name);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }

    void RayCastForPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit object name: " + hit.transform.name);

            if (hit.transform.CompareTag("Player"))
            {
                AIHealth target = hit.transform.GetComponent<AIHealth>();
                if (target != null)
                {
                    target.Takedamage(damage);
                }
                else
                {
                    Debug.LogError("AIHealth component is missing on the hit object: " + hit.transform.name);
                }
            }
            else
            {
                Debug.Log("Hit object is not tagged as Player.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object within range.");
        }
    }
}