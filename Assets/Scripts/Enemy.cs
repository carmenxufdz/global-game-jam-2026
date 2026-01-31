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

    [Header("Attributes")]
    [SerializeField] protected int speed;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;

    [SerializeField] protected float attackRange;
    [SerializeField] protected EnemyType type;

    protected bool canAttack;
    
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(gameManager.GetComponent<MaskManager>().mask)
        {
            Attack();

                    // CAMBIAR DIRECCIÓN SPRITE
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }
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

}
