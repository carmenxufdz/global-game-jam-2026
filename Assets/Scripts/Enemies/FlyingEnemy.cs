using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        type = EnemyType.Flying;
    }
    override protected void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(canAttack){
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed * Time.deltaTime;
        }
        else{
            rb.velocity = new Vector2(0,0);
        }
    }
}
