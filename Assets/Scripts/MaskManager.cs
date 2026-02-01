using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [SerializeField] GameObject lightWorld;
    [SerializeField] GameObject shadowWorld;
    [SerializeField] GameObject shadowEnemys;
    [SerializeField] GameObject lightSlider;
    [SerializeField] GameObject shadowSlider;
    [SerializeField] GameObject player;

    [SerializeField] GameObject enemiesManager;

    public bool gotMask = false;
    public bool mask = false;
    public bool canAct = true;

    public AudioSource audioManager;
    [SerializeField] AudioClip switchWorldClip;

    void Start()
    {
        mask = false;
        ActualizarMundo();
        audioManager = GameObject.FindGameObjectWithTag("SoundM").GetComponent<AudioSource>();
    }

    void Update()
    {
        // Pulsar E para cambiar de estado
        if (Input.GetKeyDown(KeyCode.E) && canAct && gotMask)
        {
            StartCoroutine(CambiarMundo());
        }
    }

    IEnumerator CambiarMundo()
    {
        canAct = false; // bloqueamos acciones mientras dura el "cambio"

        // Insertar animaci�n del jugador (si eso...)
        audioManager.PlayOneShot(switchWorldClip);

        if (!mask)
            player.GetComponent<PlayerController>().GetAnimator().CrossFade("PutOnMask", 1.0f);
        else
            player.GetComponent<PlayerController>().GetAnimator().CrossFade("PutOffMask", 1.0f);
        
        yield return new WaitForSeconds(1f); // espera X segundos durante la animaci�n
        

        // Cambiamos la m�scara y actualizamos el mundo despu�s del delay
        mask = !mask;
        ActualizarMundo();
        enemiesManager.GetComponent<EnemiesManager>().DestroyEnemies();
        enemiesManager.GetComponent<EnemiesManager>().GenerateEnemies();

        canAct = true; // desbloqueamos acciones
    }

    public void ActualizarMundo()
    {
        // Mundos
        lightWorld.SetActive(!mask);
        shadowWorld.SetActive(mask);
        lightSlider.SetActive(!mask);
        shadowSlider.SetActive(mask);
    }


}
