using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyBrain : MonoBehaviour
{
    public GameObject Player;
    public float Distance;
    public bool isAngered;
    public NavMeshAgent _agent;
    public float shootInterval = 1.5f; // Time between shots
    private float nextShootTime;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        nextShootTime = Time.time;
    }

    void Update()
    {
        Distance = Vector3.Distance(Player.transform.position, this.transform.position);
        isAngered = Distance <= 50f;
        if (isAngered)
        {
            _agent.isStopped = false;
            _agent.SetDestination(Player.transform.position);

            if (Time.time >= nextShootTime)
            {
                ShootAtPlayer();
                nextShootTime = Time.time + shootInterval;
            }
        }
        else
        {
            _agent.isStopped = true;
        }
    }
    void ShootAtPlayer()
    {
        
        PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}



