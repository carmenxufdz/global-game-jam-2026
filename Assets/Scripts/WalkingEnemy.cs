using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField] private Transform groundCheck;
    private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    override protected void Attack()
    {
        if(canAttack && HasGroundAhead()){
            print("Moving towards player...");
            Vector2 direction = player.transform.position - transform.position;
            direction = direction.normalized;

            rb.velocity = new Vector2( direction.x * speed * Time.deltaTime, 0f);
        }
        else{
            rb.velocity = new Vector2(0,0);
        }
    }

    bool HasGroundAhead()
    {
        // Devuelve true si detecta suelo
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }


}
