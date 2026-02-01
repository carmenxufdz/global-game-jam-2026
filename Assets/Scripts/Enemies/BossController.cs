using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossController : MonoBehaviour
{

    private int maxHealth;
    private int currentHealth;
    private 
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = 1;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            animator.SetBool("Dead",true);
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
