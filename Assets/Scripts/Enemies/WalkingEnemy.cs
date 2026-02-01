using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    bool attackSoundPlayed;

    [SerializeField] private Transform groundCheck;
    private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        type = EnemyType.Walking;
        animator = GetComponent<Animator>();
        hit = false;
        damage = 10;
        playerHealed = 5;
        speed = 200;
        health = 30;
        
    }
    override protected void Attack()
    {
        if(canAttack && HasGroundAhead() && !hit){
            Vector2 direction = player.transform.position - transform.position;
            direction = direction.normalized;

            rb.velocity = new Vector2( direction.x * speed * Time.deltaTime, 0f);
            if (!attackSoundPlayed)
            {
                audioManager.PlayOneShot(attackClip);
                attackSoundPlayed = true;
            }
        }
        else{
            rb.velocity = new Vector2(0,0);
            attackSoundPlayed = false;
        }
    }

    bool HasGroundAhead()
    {
        // Devuelve true si detecta suelo
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    protected override void AnimationManager()
    {
        animator.SetBool("mask", gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50);
    }

}
