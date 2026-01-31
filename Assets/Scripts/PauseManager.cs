using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{   
    [SerializeField] Transform pauseMenu;

    public void Start()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            print("Tecla pulsada");
            //Si ya está activado 
            if(pauseMenu.gameObject.activeSelf)
            {   
                Time.timeScale = 1; //Se reanuda
                pauseMenu.gameObject.SetActive(false);
            }
            else 
            {   
                Time.timeScale = 0; //Se pausa todo
                pauseMenu.gameObject.SetActive(true);
            }
        }
    }

    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    public void Retry()
    {
        //Se reinicia el nivel actual
    }

    public void Home()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
