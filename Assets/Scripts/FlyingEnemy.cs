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
        rb.velocity = new Vector2(-speed, 0);
    }
}
