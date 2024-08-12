using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isHit = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Takedamage(float amount)
    {
        currentHealth -= amount;
        isHit = true;


        if(currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            StartCoroutine(ResetIsHit());
        }

        IEnumerator ResetIsHit()
        {
            yield return new WaitForSeconds(0.5f);
            isHit = false;
        }


        void Die()
        {
            Debug.Log("Enemy Died");
            Destroy(gameObject);
        }
    }





}
