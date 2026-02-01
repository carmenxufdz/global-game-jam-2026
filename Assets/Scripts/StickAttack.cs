using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAttack : MonoBehaviour
{

    private int damage = 10;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50){
            print(collision.gameObject.tag);
            if(collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
            if (collision.gameObject.CompareTag("Boss"))
            {
                collision.gameObject.GetComponent<BossController>().TakeDamage(damage);
            }
        }
    }
}
