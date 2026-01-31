using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject player;
    [SerializeField] MaskManager maskManager;
    [SerializeField] GameObject gameManager;
    GameObject child;

    [SerializeField] GameObject childPrefab;
    // Start is called before the first frame update
    public void Init(GameObject player, GameObject gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;

        child = Instantiate(childPrefab, transform.position, Quaternion.identity, transform);
        child.SetActive(true);                // Asegura que el GameObject está activo
        child.GetComponent<RushingEnemy>().enabled = true; // Asegura que el script está activo
        child.GetComponent<RushingEnemy>().Init(player, gameManager);
        child.GetComponent<RushingEnemy>().ManagerSet(gameObject);
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public GameObject GetGameManager()
    {
        return gameManager;
    }

    public GameObject GetChild()
    {
        return child;
    }


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    public void ResetEnemy()
    {

        if(child.GetComponent<Enemy>().GetHealth() <= 0)
        {   
            Destroy(child.gameObject);
            child = Instantiate(childPrefab, transform.position, Quaternion.identity, transform);
            child.SetActive(true);                // Asegura que el GameObject está activo
            child.GetComponent<RushingEnemy>().enabled = true; // Asegura que el script está activo
            child.GetComponent<RushingEnemy>().Init(player, gameManager);
            child.GetComponent<RushingEnemy>().ManagerSet(gameObject);
        }

    }

}
