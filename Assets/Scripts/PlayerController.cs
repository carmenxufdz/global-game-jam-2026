using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int speed; //variable para el valor de la velocidad de movimiento
    [SerializeField] int jumpForce; //variable para el valor de la velocidad de salto
    [SerializeField] int hopForce; //variable para el valor de la velocidad de rebote
    [SerializeField] bool isFloored; //variable para comprobar si toca el suelo
    [SerializeField] bool isDead;
    Rigidbody2D rb;                 //referencia a rigidBody2D
    CapsuleCollider2D capsule;      //Referencia a un collider 2D de capsula

    public float sanityActual;
    public float sanityMax;

    public GameObject weapon;
    public Transform armPivot;
    private bool isAttacking=false;

    // Start is called before the first frame update
    void Start()
    {
        speed = 3;                  //le damos valor a la variable velocidad
        jumpForce = 10;                  //le damos valor a la variable salto
        hopForce = jumpForce / 2;
        rb = GetComponent<Rigidbody2D>(); //Igualamos rigidbody al Rigidbody de nuestro personaje.

        capsule = GetComponent<CapsuleCollider2D>();//nuestro collider
        sanityMax = 100;
        sanityActual = sanityMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            //usando el rigidbody, le damos velocidad en  un vector2 usando el valor que nos devuelve el eje horizontal---
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

            //salto
            if (Input.GetKeyDown(KeyCode.Space) && isFloored)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isFloored = false;
            }

            //comprueba que nuestra velocidad en x sea mayor de cero, por lo que se mueve a la derecha
            if (rb.velocity.x > 0)
            {
                //ponemos nuestra escala en x:1 e Y:1
                transform.localScale = new Vector2(1, 1);
            }
            //si no, comprueba que nuestra velocidad en x sea menor de cero, por lo que se mueve a la izquierda
            else if (rb.velocity.x < 0)
            {
                //ponemos nuestra escala en x:-1 e Y:1, dandose la vuelta
                transform.localScale = new Vector2(-1, 1);
            }

            attackControll();
        }
    }


    //este método controla colisiones SOLIDAS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //comprueba que colisionas contra un objeto que tiene como tag Enemy
        if (collision.gameObject.tag == "Enemy")
        {
            /*
            //cambio la variable de animacion muerto a true
            isDead = true;

            //desactivamos el capsule collider
            capsule.enabled = false;

            rb.velocity = new Vector2(0, 15);

            //A los 2 segundos reinica la escena
            Invoke("reinicio", 2f);*/
            //daña al jugador
            //si vida es 0 = muerte
        }
    }

    //este método controla colisiones NO SOLIDAS (trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //comprueba que tocas un objeto del nombre item
        if (collision.gameObject.tag == "Item")
        {
            print("has tocado el item");

            //destruye el objeto contra el que colisionas
            Destroy(collision.gameObject);
        }
        /*
        //comprueba que colisionas contra un objeto que tiene como tag Enemy
        if (collision.gameObject.tag == "Enemy")
        {
            //destruye contra quien colisionas
            Destroy(collision.gameObject);

            //da un movimiento hacia arriba, usando la variable FuerzaRebote
            rb.velocity = new Vector2(rb.velocity.x, hopForce);

        }*/

        if (collision.gameObject.tag == "Suelo")
        {
            //Ponemos la variable en true
            isFloored = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Suelo")
        {
            isFloored = false;
        }
    }

    void reinicio()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void attackControll()
    {
        //el jugador ataca cuando pulsa el click izquierdo del ratón
        if(Input.GetMouseButton(0) && !isAttacking)
        {
            StartCoroutine(playerAttack());
        }
    }

    IEnumerator playerAttack()
    {
        isAttacking = true;
        weapon.SetActive(true);

        float startAngle = 60f;   // arriba
        float endAngle = -60f;    // abajo
        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, time / duration);
            armPivot.localRotation = Quaternion.Euler(0, 0, angle);
            time += Time.deltaTime;
            yield return null;
        }

        armPivot.localRotation = Quaternion.identity;
        weapon.SetActive(false);
        isAttacking = false;
    }
}
