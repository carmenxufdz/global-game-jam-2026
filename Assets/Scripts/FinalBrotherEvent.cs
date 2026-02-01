using System.Collections;
using UnityEngine;

public class FinalBrotherEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject brother; 
    [SerializeField] GameObject boss;  
    [SerializeField] Sprite newBrotherSprite; 
    [SerializeField] float waitAfterBossDeath; 

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
        // 1️⃣ Espera unos segundos
        yield return new WaitForSeconds(waitAfterBossDeath);
        Destroy(boss);
        
        // 2️⃣ Cambiar sprite del hermano
        if (brother != null && newBrotherSprite != null)
        {
            SpriteRenderer sr = brother.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = newBrotherSprite;
            }
        }

        // 3️⃣ Mover hermano a la posición del boss
        if (brother != null && boss != null)
        {
            brother.transform.position = boss.transform.position;
        }
    }
}
