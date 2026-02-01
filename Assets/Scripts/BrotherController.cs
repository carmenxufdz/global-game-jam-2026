using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherController : MonoBehaviour
{
    private int speed = 4;
    private Rigidbody2D rb;
    public bool canMove;

    // Update is called once per frame
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    public void Walk(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
}
