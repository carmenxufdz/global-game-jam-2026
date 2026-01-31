using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemy : Enemy
{
    private Vector2 direction;
    [SerializeField] GameObject rushingEnemyManager;
    
    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        type = EnemyType.Rushing;
        speed = 1000;
        canAttack = true;
    }


    public void Init(GameObject player, GameObject gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;

        // Aquí puedes inicializar todo lo que depende de estas referencias
    }


    override protected void Attack()
    {
        if (canAttack)
        {
            //rb.velocity = direction * speed * Time.deltaTime; //
            print("te ataco");
            StartCoroutine(WaitForAnimation());
        }
        else
            rb.velocity = new Vector2(0, 0);
    }

    IEnumerator WaitForAnimation()
    {
        print("ready?");
        yield return new WaitForSeconds(2f);//cambiar el 2f por la duración de la animación
        rb.velocity = direction * speed * Time.deltaTime;
        print("shoot");
    }


    override protected void OnBecameVisible()
    {
       direction = (player.transform.position - transform.position).normalized;
       canAttack = true; 
    }

    override protected void OnBecameInvisible()
    {
        if(health <= 0)
        {   
            if(canAttack)
                rushingEnemyManager.GetComponent<RushingEnemyManager>().ResetEnemy();
        }
    }

    public void ManagerSet(GameObject manager)
    {
        rushingEnemyManager = manager;
    }

    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        print("Rushing enemy trigger enter");
        if (collision.gameObject.CompareTag("Player") && gameManager.GetComponent<MaskManager>().mask)
        {
            print("Player hit rushing enemy");
            hit = true;
            player.GetComponent<PlayerController>().TakeDamage(damage);
            rushingEnemyManager.GetComponent<RushingEnemyManager>().ResetEnemy();
        }
    }

    protected override void AnimationManager()
    {
        // No animation for rushing enemy
    }
}
