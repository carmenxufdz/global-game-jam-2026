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
    private Vector2 bossPosition;

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
        bossPosition = boss.transform.position;
        Destroy(boss);

        brother.SetActive(false);
        yield return new WaitForSeconds(brotherSpawningTime);
        if (brother != null)
        {
            brother.transform.position = bossPosition;
        }
        if (brother != null && newBrotherSprite != null)
        {
            SpriteRenderer sr = brother.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = newBrotherSprite;
            }
        }
        brother.SetActive(true);

        yield return new WaitForSeconds(quittingTime);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
