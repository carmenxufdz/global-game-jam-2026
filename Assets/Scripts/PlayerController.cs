using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int speed; //variable para el valor de la velocidad de movimiento
    [SerializeField] int jumpForce; //variable para el valor de la velocidad de salto
    [SerializeField] bool isGrounded; //variable para comprobar si toca el suelo
    [SerializeField] bool isDead;
    Rigidbody2D rb;                 //referencia a rigidBody2D

    Animator animator;
    public Slider lightLifeSlider;       //slider de la vida/sanidad del jugador
    public Slider shadowLifeSlider;       //slider de la vida/sanidad del jugador

    public float currentSanity;
    public float maxSanity;

    public GameObject weapon;
    public Transform armPivot;
    private bool isAttacking=false;

    MaskManager maskManager;

    //invulnerability
    public float invulnerabilityTime = 2f;
    public float blinkInterval = 0.1f;

    bool isInvulnerable = false;
    
    void Start()
    {
        speed = 4;                  
        jumpForce = 12;      
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3;

        maxSanity = 100;
        currentSanity = maxSanity;

        maskManager= GameObject.FindGameObjectWithTag("GameController").GetComponent<MaskManager>();

        lightLifeSlider.maxValue = maxSanity;
        shadowLifeSlider.maxValue = maxSanity;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            //usando el rigidbody, le damos velocidad en  un vector2 usando el valor que nos devuelve el eje horizontal---
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isGrounded = false;
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

            lightLifeSlider.value = currentSanity;
            shadowLifeSlider.value = currentSanity;

            AttackControll();
            inShadowWorld();

            if(rb.velocity.x != 0 && isGrounded)
            {
                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);
            }
        }
    }

    //este m�todo controla colisiones NO SOLIDAS (trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //comprueba que tocas un objeto del nombre item
        if (collision.gameObject.tag == "Item")
        {
            print("has tocado el item");

            //destruye el objeto contra el que colisionas
            Destroy(collision.gameObject);
        }
    
        if (collision.gameObject.tag == "Floor")
        {
            //Ponemos la variable en true
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void AttackControll()
    {
        //el jugador ataca cuando pulsa el click izquierdo del raton
        if(Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(PlayerAttack());
        }
    }

    IEnumerator PlayerAttack()
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

    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
            return; // no recibe daño

        currentSanity -= damage;

        if (currentSanity <= 0)
        {
            // muerte
            return;
        }

        //StartCoroutine(Invulnerability());

    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(blinkInterval);
        isInvulnerable = false;
    }




    public void PlayerHealed(float health)
    {
        currentSanity += health;
    }

    Coroutine sanityCoroutine;

    void inShadowWorld()
    {
        if (maskManager.mask && sanityCoroutine == null)
        {
            animator.SetBool("mask", true);
            sanityCoroutine = StartCoroutine(LoseSanity());
        }
        else if (!maskManager.mask && sanityCoroutine != null)
        {
            animator.SetBool("mask", false);
            StopCoroutine(sanityCoroutine);
            sanityCoroutine = null;

            // opcional: reset al quitar la máscara
            reductionSpeed = 0.1f;
        }
    }

    private float reductionSpeed = 0.1f;
    private float reductionAccel = 0.05f;
    public float maxReductionSpeed = 1.5f;

    IEnumerator LoseSanity()
    {
        yield return new WaitForSeconds(3f);

        while (maskManager.mask)
        {
            reductionSpeed += reductionAccel * Time.deltaTime;
            reductionSpeed = Mathf.Min(reductionSpeed, maxReductionSpeed);

            currentSanity -= reductionSpeed * Time.deltaTime;
            currentSanity = Mathf.Max(currentSanity, 0f);

            yield return null;
        }

        sanityCoroutine = null;
    }
}
