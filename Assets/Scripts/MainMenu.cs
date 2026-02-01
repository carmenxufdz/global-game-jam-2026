using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    //audio
    [SerializeField] AudioClip select;
    AudioSource audio;

    void Start()
    {
        audio = GameObject.FindGameObjectWithTag("SoundM").GetComponent<AudioSource>(); //awake
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        audio.PlayOneShot(select);
        SceneManager.LoadScene("GameScene");
    }

    public void HomeButton()
    {
        audio.PlayOneShot(select);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void QuitGame()
    {
        audio.PlayOneShot(select);
        Application.Quit();
    }
}
