using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum UIType {Gameplay, Pause, GameOver}
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject hudUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject player;
    private UIType currentMenu;

    //audio
    [SerializeField] AudioClip select;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = UIType.Gameplay;
        audio = GameObject.FindGameObjectWithTag("SoundM").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentMenu)
        {
            case UIType.Gameplay:
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                hudUI.SetActive(true);
                pauseUI.SetActive(false);
                gameOverUI.SetActive(false);
                
                if(Input.GetKeyDown(KeyCode.Escape))
                { 
                    currentMenu = UIType.Pause;
                }

                if(player.GetComponent<PlayerController>().currentSanity <= 0)
                {
                    currentMenu = UIType.GameOver;
                }
                break;
            case UIType.Pause:
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                hudUI.SetActive(false);
                pauseUI.SetActive(true);
                gameOverUI.SetActive(false);
                
                if(Input.GetKeyDown(KeyCode.Escape))
                { 
                    currentMenu = UIType.Gameplay;
                }
                break;
            case UIType.GameOver:
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                hudUI.SetActive(false);
                pauseUI.SetActive(false);
                gameOverUI.SetActive(true);
                break;
            default:
                break;
        }
    }
    public UIType GetUIType() => currentMenu;
    public void ResumeButton()
    {
        audio.PlayOneShot(select);
        currentMenu = UIType.Gameplay;
    }

    public void RestartButton()
    {
        audio.PlayOneShot(select);
        currentMenu = UIType.Gameplay;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HomeButton()
    {
        print("HOome pulsado");
        audio.PlayOneShot(select);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
