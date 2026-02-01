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

    public bool mask = false;
    public bool canAct = true;

    void Start()
    {
        mask = false;
        ActualizarMundo();
    }

    void Update()
    {
        // Pulsar E para cambiar de estado
        if (Input.GetKeyDown(KeyCode.E) && canAct)
        {
            StartCoroutine(CambiarMundo());
        }
    }

    IEnumerator CambiarMundo()
    {
        canAct = false; // bloqueamos acciones mientras dura el "cambio"

        // Insertar animaci�n del jugador (si eso...)

        if(!mask)
            player.GetComponent<PlayerController>().GetAnimator().CrossFade("PutOnMask", 1.0f);
        else
            player.GetComponent<PlayerController>().GetAnimator().CrossFade("PutOffMask", 1.0f);
        
        yield return new WaitForSeconds(1f); // espera X segundos durante la animaci�n
        

        // Cambiamos la m�scara y actualizamos el mundo despu�s del delay
        mask = !mask;
        ActualizarMundo();

        canAct = true; // desbloqueamos acciones
    }

    void ActualizarMundo()
    {
        // Mundos
        lightWorld.SetActive(!mask);
        shadowWorld.SetActive(mask);
        lightSlider.SetActive(!mask);
        shadowSlider.SetActive(mask);
    }


}
