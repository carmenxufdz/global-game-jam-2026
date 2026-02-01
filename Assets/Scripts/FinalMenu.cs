using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReturnToMainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReturnToMainMenu()
    {

        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
