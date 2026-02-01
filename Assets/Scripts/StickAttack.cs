using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAttack : MonoBehaviour
{

    private int damage = 10;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameManager.GetComponent<MaskManager>().mask || player.GetComponent<PlayerController>().GetHealth() <50){
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
