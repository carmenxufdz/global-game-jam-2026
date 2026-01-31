using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    bool attackSoundPlayed;

    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        type = EnemyType.Flying;
        health = 20;
        speed = 300;
        damage = 5;
        animator = GetComponent<Animator>();
    }
    override protected void Attack()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(canAttack){
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed * Time.deltaTime;
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

    protected override void AnimationManager()
    {
        animator.SetBool("mask", gameManager.GetComponent<MaskManager>().mask);
    }
}
