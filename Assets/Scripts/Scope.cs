using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scope : MonoBehaviour
{
    public Animator animator;

    private bool isScoped = false;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Q))
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);
        }
    }
}
