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
        ActualizarEnemigos();
    }

    void Update()
    {
        // Pulsar E para cambiar de estado
        if (Input.GetKeyDown(KeyCode.E) && canAct)
        {
            StartCoroutine(CambiarMundo());
        }

        ActualizarEnemigos();
    }

    IEnumerator CambiarMundo()
    {
        canAct = false; // bloqueamos acciones mientras dura el "cambio"

        // Insertar animación del jugador (si eso...)

        yield return new WaitForSeconds(2f); // espera X segundos durante la animación

        // Cambiamos la máscara y actualizamos el mundo después del delay
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

    void ActualizarEnemigos()
    {
        // Enemigos sombra:
        // activos si máscara = true O vida < 50%
        if (mask || player.sanityActual < player.sanityMax / 2f)
        {
            shadowEnemys.SetActive(true);
        }
        else
        {
            shadowEnemys.SetActive(false);
        }
    }
}
