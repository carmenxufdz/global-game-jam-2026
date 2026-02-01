using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBrotherEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MaskManager maskManager;
    [SerializeField] GameObject brother; 
    [SerializeField] GameObject boss;  
    [SerializeField] Sprite newBrotherSprite; 
    private float bossDeathTime = 1.7f; 
    private float quittingTime = 5;
    private Vector2 bossPosition;
    Animator bossAnimator;

    private bool eventTriggered = false;

    void Start()
    {
        bossAnimator = boss.GetComponent<Animator>();
    }
    void Update()
    {
        if (!eventTriggered && boss.GetComponent<BossController>().GetCurrentHealth() <= 0)
        {
            eventTriggered = true;
            bossAnimator.SetBool("Dead", true);
            StartCoroutine(BrotherAfterBossRoutine());
        }
    }

    private IEnumerator BrotherAfterBossRoutine()
    {

        
        yield return new WaitForSeconds(2f);

        // Guardamos la posición del boss antes de destruirlo
        bossPosition = boss.transform.position;

        // Cambiamos el mundo justo después de que muere el boss
        if(maskManager != null)
        {
            maskManager.mask = false;
            maskManager.ActualizarMundo();
        }

        // Destruimos el boss sin que se vea el último frame
        boss.SetActive(false);
        Destroy(boss);

        // Posicionamos y preparamos al brother
        if (brother != null)
        {
            brother.SetActive(false); // aseguramos que no se vea aún
            brother.transform.position = new Vector2(bossPosition.x, -2.5f);

            if (newBrotherSprite != null)
            {
                SpriteRenderer sr = brother.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = newBrotherSprite;
            }

            // Activamos al brother en el siguiente frame
            yield return null;
            brother.SetActive(true);
        }

        // Espera antes de volver al menú
        yield return new WaitForSeconds(quittingTime);
        SceneManager.LoadSceneAsync("GameFinishScene");
    }

}
