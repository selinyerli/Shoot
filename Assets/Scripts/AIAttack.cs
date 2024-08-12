using System.Collections;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackRate = 1f;
    public float attackDamage = 10f;

    private float nextAttackTime = 0f;
    private Transform player;
    private PlayerHealth playerHealth;
    private AIHealth aiHealth;
    public GameObject ai;

    public GameObject attackPrefab;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();
        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found.");
        }
        else
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component not found on Player GameObject.");
            }
        }
    }

    public void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void Attack()
    {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogError("PlayerHealth is null. Cannot apply damage.");
            }
        }

        void OnDrawGizmosSelected()
        {
            if (player == null)
                return;

            if (aiHealth != null && aiHealth.isHit)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawWireSphere(transform.position, attackRange);

        }
    } 