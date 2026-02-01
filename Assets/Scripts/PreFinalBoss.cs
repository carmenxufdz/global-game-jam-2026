using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreFinalBoss : MonoBehaviour
{   
    [SerializeField] MaskManager maskManager;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            maskManager.mask = true;
            maskManager.ActualizarMundo();   
        }
    }
}
