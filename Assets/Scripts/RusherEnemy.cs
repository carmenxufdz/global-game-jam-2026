using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemy : Enemy
{
    [SerializeField] Transform player;
    private bool canAttack;
    override protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    override protected void Attack()
    {
        float distanceToPlayer = transform.position.x - player.transform.position.x;
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if(canAttack)
            rb.velocity = direction * speed * Time.deltaTime;
        else
            rb.velocity = new Vector2(0,0);
    }

    void OnBecameVisible()
    {
       canAttack = true; 
    }

    void OecameInvisible()
    {
        canAttack = false;
    }
}
