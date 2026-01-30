using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    [SerializeField] protected int speed;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;

    [SerializeField] protected GameObject gameManager;
    
    protected Rigidbody2D rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.GetComponent<MaskManager>().mask)
        {
            
            Attack();
        }
    }

    protected abstract void Attack();


}
