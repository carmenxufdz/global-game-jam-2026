using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private int speed;
    private int baseSpeed = 4;
    [SerializeField] int jumpForce; //variable para el valor de la velocidad de salto
    bool isDead;
    bool onGround;
    [SerializeField] GameObject uiManager;

    [SerializeField] private Transform groundCheck;

    private float groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    Rigidbody2D rb;                 //referencia a rigidBody2D

    Animator animator;
    public Slider lightLifeSlider;       //slider de la vida/sanidad del jugador
    public Slider shadowLifeSlider;       //slider de la vida/sanidad del jugador

    public float currentSanity;
    public float maxSanity;

    public GameObject weapon;
    public Transform armPivot;
    private bool isAttacking=false;
    private bool isInEvent = false ;

    MaskManager maskManager;

    [SerializeField] float thornDamageInterval = 1f;
    Coroutine thornRoutine;

    private float invulnerabilityTime = 2f;
    private float thornDamage = 15;

    bool isInvulnerable = false;

    //audio
    AudioSource audio;
    AudioSource audioManager;
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip hurtClip;

    void Start()
    {
        speed = baseSpeed;                  
        jumpForce = 12;      
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3;

        maxSanity = 100;
        currentSanity = maxSanity;

        maskManager= GameObject.FindGameObjectWithTag("GameController").GetComponent<MaskManager>();

        lightLifeSlider.maxValue = maxSanity;
        shadowLifeSlider.maxValue = maxSanity;
        animator = GetComponent<Animator>();

        //
        audioManager = GameObject.FindGameObjectWithTag("SoundM").GetComponent<AudioSource>();
        audio = GetComponent<AudioSource>();
        audio.clip = walkClip;
        audio.loop = true;
    }

    void Update()
    {   
        if (!isDead && !isInEvent)
        {   
            //usando el rigidbody, le damos velocidad en  un vector2 usando el valor que nos devuelve el eje horizontal---
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

            CheckGround();

            if (Input.GetKeyDown(KeyCode.Space) && onGround)
            {
                audioManager.PlayOneShot(jumpClip);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

            if(rb.velocity.x != 0 && onGround)
            {
                if (!audio.isPlaying)
                    audio.Play();
                animator.SetBool("walking", true);
            }
            else
            {
                audio.Stop();
                animator.SetBool("walking", false);
            }
        }else{
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("walking", false);
                audio.Stop();
                return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Thorn"))
        {
            speed = baseSpeed / 2;

            if (thornRoutine == null)
                thornRoutine = StartCoroutine(ThornDamageRoutine());
        }
        if(collision.CompareTag("Mask"))
        {
            Destroy(collision.gameObject);
            maskManager.gotMask = true;
            //NotificationManager.Instance.Show("Press E to wear the mask");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Thorn"))
        {
            speed = baseSpeed;

            if (thornRoutine != null)
            {
                StopCoroutine(thornRoutine);
                thornRoutine = null;
            }
        }
    }

    void CheckGround()
    {
        // Lanzamos el raycast
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        onGround = hit.collider != null;

        animator.SetBool("jumping", !onGround);

    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        // Lanzamos un raycast visual para mostrar hasta dónde llega
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        Vector3 start = groundCheck.position;
        Vector3 end = hit.collider != null ? (Vector3)hit.point : groundCheck.position + Vector3.down * groundCheckDistance;

        // Color según si impacta o no
        Gizmos.color = hit.collider != null ? Color.green : Color.red;

        // Línea del raycast
        Gizmos.DrawLine(start, end);

        // Pequeño punto en el final del raycast
        Gizmos.DrawSphere(end, 0.05f);
    }

    void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void AttackControll()
    {
        //el jugador ataca cuando pulsa el click izquierdo del raton
        if(Input.GetMouseButtonDown(0) && !isAttacking && uiManager.GetComponent<UIManager>().GetUIType() == UIType.Gameplay)
        {
            StartCoroutine(PlayerAttack());
        }
    }

    IEnumerator PlayerAttack()
    {
        isAttacking = true;
        weapon.SetActive(true);
        audioManager.PlayOneShot(attackClip);

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
        audioManager.PlayOneShot(hurtClip);

        if (currentSanity <= 0)
        {
            return;
        }
        StartCoroutine(Invulnerability());
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
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
    
    public float GetHealth() => currentSanity;

    public Animator GetAnimator() => animator;

    //Para eventos de pararse a ver algo
    public void FreezeForSeconds(float time)
    {
        StartCoroutine(FreezeRoutine(time));
    }

    IEnumerator FreezeRoutine(float time)
    {
        isInEvent = true;
        yield return new WaitForSeconds(time);
        isInEvent = false;
    }

    IEnumerator ThornDamageRoutine()
    {
        while (true)
        {
            TakeDamage(thornDamage);

            audioManager.PlayOneShot(hurtClip);

            yield return new WaitForSeconds(thornDamageInterval);
        }
    }
}
