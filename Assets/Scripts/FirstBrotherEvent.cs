using System.Collections;
using UnityEngine;

public class FirstBrotherEvent : MonoBehaviour
{
    [SerializeField] float playerFreezeTime = 6f;
    [SerializeField] BrotherController brotherNPC; // puede estar vacío
    [SerializeField] float npcWaitTime = 3f;
    [SerializeField] Vector2 npcWalkDirection = Vector2.right;
    [SerializeField] float npcWalkDuration = 3f; // segundos que camina antes de destruirse

    
    AudioSource audioManager;
    [SerializeField] AudioClip meetingClip;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("SoundM").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.FreezeForSeconds(playerFreezeTime);
            }

            if (brotherNPC != null)
            {

                audioManager.PlayOneShot(meetingClip);
                StartCoroutine(NPCEventRoutine(brotherNPC));
            }

            GetComponent<Collider2D>().enabled = false; // solo se activa una vez
        }
    }

    private IEnumerator NPCEventRoutine(BrotherController npc)
    {
        // 1️⃣ Parado
        npc.Walk(Vector2.zero);   // se queda quieto
        yield return new WaitForSecondsRealtime(npcWaitTime);

        // 2️⃣ Caminar
        npc.Walk(npcWalkDirection); // empieza a moverse

        yield return new WaitForSecondsRealtime(npcWalkDuration);

        // 4️⃣ Destruir NPC
        Destroy(npc.gameObject);
    }
}
