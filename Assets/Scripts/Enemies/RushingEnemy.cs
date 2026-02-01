using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemy : Enemy
{
    private Vector2 direction;
    [SerializeField] GameObject rushingEnemyManager;
    [SerializeField] AudioClip launchClip;

    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        type = EnemyType.Rushing;
        speed = 1000;
        canAttack = true;
        health = 10;
        damage = 1;
        animator = GetComponent<Animator>();
    }


    override protected void Attack()
    {
        if (canAttack)
        {
            //rb.velocity = direction * speed * Time.deltaTime; //
            StartCoroutine(WaitForAnimation());
        }
        else
            rb.velocity = new Vector2(0, 0);
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1.0f);//cambiar el 2f por la duración de la animación
        rb.velocity = direction * speed * Time.deltaTime;
        this.audioManager.PlayOneShot(launchClip);
    }


    override protected void OnBecameVisible()
    {
       direction = (player.transform.position - transform.position).normalized;
       canAttack = true; 
    }

    override protected void OnBecameInvisible()
    {
        if(canAttack)
            rushingEnemyManager.GetComponent<RushingEnemyManager>().ResetEnemy();
    }

    public void ManagerSet(GameObject manager)
    {
        rushingEnemyManager = manager;
    }

    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50)
        {
            print("Player hit rushing enemy");
            hit = true;
            player.GetComponent<PlayerController>().TakeDamage(damage);
            rushingEnemyManager.GetComponent<RushingEnemyManager>().ResetEnemy();
        }
    }

    protected override void AnimationManager()
    {
        animator.SetBool("mask", gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50);
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            player.GetComponent<PlayerController>().PlayerHealed(10);
            rushingEnemyManager.GetComponent<RushingEnemyManager>().ResetEnemy();
        }
    }
}
