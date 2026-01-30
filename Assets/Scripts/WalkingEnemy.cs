using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    override protected void Attack()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }
}
