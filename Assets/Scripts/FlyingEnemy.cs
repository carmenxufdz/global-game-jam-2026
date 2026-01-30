using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    override protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    override protected void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance <= attackRange){
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else{
            rb.velocity = Vector2.zero;
        }
    }
}
