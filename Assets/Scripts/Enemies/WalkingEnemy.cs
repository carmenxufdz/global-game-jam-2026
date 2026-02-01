using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField] private Transform groundCheck;
    private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        type = EnemyType.Walking;
        animator = GetComponent<Animator>();
        hit = false;
        damage = 5;
        speed = 200;
        health = 30;
        
    }
    override protected void Attack()
    {
        if(canAttack && HasGroundAhead() && !hit){
            Vector2 direction = player.transform.position - transform.position;
            direction = direction.normalized;

            rb.velocity = new Vector2( direction.x * speed * Time.deltaTime, 0f);
            this.audioManager.PlayOneShot(this.attackClip);
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

    protected override void AnimationManager()
    {
        animator.SetBool("mask", gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50);
    }

}
