using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemy : Enemy
{
    bool attackSoundPlayed;

    private Vector2 direction;
    [SerializeField] GameObject rushingEnemyManager;
    [SerializeField] AudioClip launchClip;

    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        type = EnemyType.Rushing;
        speed = 8;
        canAttack = true;
        health = 10;
        damage = 1;
        playerHealed = 0;
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
        {
            rb.velocity = new Vector2(0, 0);
            attackSoundPlayed = false;
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1.0f);//cambiar el 2f por la duración de la animación
        rb.velocity = direction * speed;
        if (!attackSoundPlayed)
        {
            audioManager.PlayOneShot(launchClip);
            attackSoundPlayed = true;
        }
    }


    override protected void OnBecameVisible()
    {
        direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
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
        if (collision.gameObject.CompareTag("Player") && (gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50))
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
            rushingEnemyManager.GetComponent<RushingEnemyManager>().ResetEnemy();
        }
    }
}
