using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    private float maxHealth;
    private RespawnManager respawnManager;
    public HealthBar healthBar;
    public bool isHit = false;

    private void Start()
    {
        maxHealth = health;      
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Target");

        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }

        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {      
            Destroy(gameObject);
    }
    public void ResetHealth()
    {
        health = maxHealth;
    }
}
