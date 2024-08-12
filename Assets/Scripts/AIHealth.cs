using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;
    public bool isHit = false;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogError("HealthBar reference is missing!");
        }
    }

    public void Takedamage(float amount)
    {
        Debug.Log("AIHealth");

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

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

    private void Die()
    {
        Debug.Log("AI Died");
        Destroy(gameObject);
    }

    private IEnumerator ResetIsHit()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }
}