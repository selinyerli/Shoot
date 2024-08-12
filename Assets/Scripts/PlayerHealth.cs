using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;
    public RespawnManager respawnManager; 

    public bool isHit = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        isHit = true;

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            StartCoroutine(ResetIsHit());
        }
    }

    IEnumerator ResetIsHit()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void Die()
    {
        Debug.Log("Player Died");
        Respawn();
    }

    void Respawn()
    {
        if (respawnManager != null)
        {
            Transform respawnPoint = respawnManager.GetRandomRespawnPoint(); 
            if (respawnPoint != null)
            {
                transform.GetComponent<CharacterController>().enabled = false;
                transform.position = respawnPoint.position; 
                currentHealth = maxHealth;
                UpdateHealthBar();
                transform.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                Debug.LogError("No respawn point found!");
            }
        }
        else
        {
            Debug.LogError("RespawnManager reference is missing!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Respawn(); 
        }
    }

    void UpdateHealthBar()
    {
        healthBar.SetHealth(currentHealth); 
    }
}