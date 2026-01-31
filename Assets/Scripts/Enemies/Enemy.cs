using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyType {Walking, Flying, Rushing}
abstract public class Enemy : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject gameManager;

    protected int speed;
    protected int health;
    protected int damage;
    protected EnemyType type;

    protected Animator animator;
    protected bool canAttack;
    
    protected Rigidbody2D rb;

    protected bool hit = false;

    protected abstract void Awake();
    protected virtual void Update()
    {
        // ActualizarSprite();
        if(gameManager.GetComponent<MaskManager>().mask)
        {
            Attack();

            // CAMBIAR DIRECCIÓN SPRITE
            if (rb.velocity.x < 0 )
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }

        AnimationManager();
    }

    public void Init(GameObject player, GameObject gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;
    }

    protected abstract void Attack();

    protected virtual void OnBecameVisible()
    {
       canAttack = true; 
    }

    protected virtual void OnBecameInvisible()
    {   
        canAttack = false;
    }

    public EnemyType GetEnemyType() => type;
    public int GetHealth() => health;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        print("Enemy trigger enter");
        if(collision.gameObject.CompareTag("Player") && gameManager.GetComponent<MaskManager>().mask)
        {
            print("Player hit enemy");
            hit = true;
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            print("Player exit enemy");
            hit = false;
        }
    }

    protected abstract void AnimationManager();

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
            player.GetComponent<PlayerController>().PlayerHealed(10);
        }
    }
}
