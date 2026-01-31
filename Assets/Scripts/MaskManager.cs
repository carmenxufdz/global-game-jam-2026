using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    public GameObject lightWorld;
    public GameObject shadowWorld;
    public GameObject shadowEnemys;

    public bool mask = false;
    public bool canAct = true;

    public PlayerController player; // referencia al script del jugador

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

        yield return new WaitForSeconds(2f); // espera X segundos durante la animaci�n

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
    }


}
