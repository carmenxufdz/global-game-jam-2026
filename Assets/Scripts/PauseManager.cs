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
        Time.timeScale = 0; //Se pausa todo

        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            //Si ya está activado 
            if(pauseMenu.gameObject.activeSelf)
            {   
                pauseMenu.gameObject.SetActive(false);
            }
            else 
            {   
                print("Se cambia a true");
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
