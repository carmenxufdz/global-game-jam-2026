using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemy : Enemy
{
    private Vector2 direction;
    
    override protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        type = EnemyType.Rushing;
        speed = 1000;
    }


    public void Init(GameObject player, GameObject gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;

        // Aquí puedes inicializar todo lo que depende de estas referencias
    }



    override protected void Attack()
    {
        if(canAttack){
            rb.velocity = direction * speed * Time.deltaTime;
        }
        else
            rb.velocity = new Vector2(0,0);
    }

    override protected void OnBecameVisible()
    {
       direction = (player.transform.position - transform.position).normalized;
       canAttack = true; 
    }

    override protected void OnBecameInvisible()
    {
        //Para que se destruya si se sale de la camara despues de haber atacado
        //if(canAttack)
            //Destroy(gameObject);

       canAttack = false; 
    }
}
