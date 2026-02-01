using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCurrentHealth() => currentHealth;
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
