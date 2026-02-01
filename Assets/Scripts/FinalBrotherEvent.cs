using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBrotherEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject brother; 
    [SerializeField] GameObject boss;  
    [SerializeField] Sprite newBrotherSprite; 
    private float bossDeathTime = 2; 
    private float brotherSpawningTime = 1;
    private float quittingTime = 5;

    private bool eventTriggered = false;

    void Update()
    {
        if (!eventTriggered && boss.GetComponent<BossController>().GetCurrentHealth() <= 0)
        {
            eventTriggered = true;
            StartCoroutine(BrotherAfterBossRoutine());
        }
    }

    private IEnumerator BrotherAfterBossRoutine()
    {
        // Espera unos segundos
        yield return new WaitForSeconds(bossDeathTime);
        Destroy(boss);

        yield return new WaitForSeconds(brotherSpawningTime);
        // Cambiar sprite del hermano
        if (brother != null && newBrotherSprite != null)
        {
            SpriteRenderer sr = brother.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = newBrotherSprite;
            }
        }

        // Mover hermano a la posición del boss
        if (brother != null && boss != null)
        {
            brother.transform.position = boss.transform.position;
        }

        yield return new WaitForSeconds(quittingTime);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
