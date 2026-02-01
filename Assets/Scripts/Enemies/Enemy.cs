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

    protected int playerHealed;
    protected EnemyType type;

    protected Animator animator;
    protected bool canAttack;
    
    protected Rigidbody2D rb;

    protected bool hit = false;

    //audio
    public AudioSource audioManager;
    [SerializeField] AudioClip deathClip;
    [SerializeField] public AudioClip attackClip;

    protected abstract void Awake();
    protected virtual void Update()
    {
        if(gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50)
        {
            Attack();
            //audioManager.PlayOneShot(attackClip);

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

    public void Init(GameObject player, GameObject gameManager, GameObject audioManager)
    {
        this.player = player;
        this.gameManager = gameManager;
        this.audioManager = audioManager.GetComponent<AudioSource>();
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
        if(collision.gameObject.CompareTag("Player") && gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50)
        {
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
        audioManager.PlayOneShot(deathClip);
        if (health <= 0)
        {
            Destroy(gameObject);
            player.GetComponent<PlayerController>().PlayerHealed(playerHealed);
        }
    }
}
