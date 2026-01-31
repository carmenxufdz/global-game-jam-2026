using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject audioManager;
    GameObject child = null;

    [SerializeField] GameObject childPrefab;
    // Start is called before the first frame update
    public void Init(GameObject player, GameObject gameManager)
    {
        this.player = player;
        this.gameManager = gameManager;

        if (child == null)
        {
            CreateChild();
        }

    }

    public void CreateChild()
    {
        child = Instantiate(childPrefab, transform.position, Quaternion.identity, transform);
        child.SetActive(true);

        RushingEnemy enemy = child.GetComponent<RushingEnemy>();
        enemy.enabled = true;
        enemy.Init(player, gameManager, audioManager);
        enemy.ManagerSet(gameObject);
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

        
        Destroy(child.gameObject);
        CreateChild();

    }

}
