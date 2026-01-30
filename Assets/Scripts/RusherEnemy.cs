using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemy : Enemy
{
    private Vector2 direction;
    
    override protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    override protected void Attack()
    {
        if(canAttack)
            rb.velocity = direction * speed * Time.deltaTime;
        else
            rb.velocity = new Vector2(0,0);
    }

    override protected void OnBecameVisible()
    {
       direction = (player.transform.position - transform.position).normalized;
       canAttack = true; 
    }
}
