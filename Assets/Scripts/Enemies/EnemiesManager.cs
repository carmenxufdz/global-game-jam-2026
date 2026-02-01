using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject enemiesLayer; //Capa de los enemigos (shadow enemies)
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject audioManager;

    [Header("Prefabs")]
    [SerializeField] GameObject walkingEnemyPrefab;
    [SerializeField] GameObject FlyingEnemyPrefab;
    [SerializeField] GameObject rushingEnemyManagerPrefab;

    private int totalEnemies;
    private Dictionary<Vector2,EnemyType> enemiesData = new();
    private Dictionary<EnemyType, GameObject> enemiesPrefabs;

    private bool hasRun = false;

    void Start()
    {   
        enemiesPrefabs = new Dictionary<EnemyType, GameObject>
        {
            {EnemyType.Walking, walkingEnemyPrefab},
            {EnemyType.Flying, FlyingEnemyPrefab},
            {EnemyType.Rushing, rushingEnemyManagerPrefab}
        };

        GetEnemiesData(); //Guardamos posicio y tipo de cada enemigo para generarlos después
    }

    void Update()
    {
        //Si se está cambiando el mundo
        if (!gameManager.GetComponent<MaskManager>().canAct && !hasRun)
        {
            StartCoroutine(DestroyEnemies()); //Eliminamos todos los enemigos
            
            StartCoroutine(GenerateEnemies()); //Los generamos de nuevo en sus posiciones originales

            hasRun = true;
        }
        // Opcional: resetear si canAct vuelve a true
        else if (gameManager.GetComponent<MaskManager>().canAct && hasRun)
        {
            hasRun = false;
        }
    }

    private void GetEnemiesData()
    {
        totalEnemies = enemiesLayer.transform.childCount;
        for(int i = 0; i < enemiesLayer.transform.childCount; i++)
        {
            GameObject enemy = enemiesLayer.transform.GetChild(i).gameObject;
            if(enemy.GetComponent<Enemy>() != null)
                enemiesData.Add(enemy.transform.position,enemy.GetComponent<Enemy>().GetEnemyType());
            else
                enemiesData.Add(enemy.transform.position,EnemyType.Rushing);
        }
    }

    private IEnumerator DestroyEnemies()
    {
        yield return new WaitForSeconds(2f); // espera X segundos durante la animaci�n
        for(int i = 0; i < enemiesLayer.transform.childCount ; i++)
        {
            if(enemiesLayer.transform.GetChild(i).gameObject != null)
                Destroy(enemiesLayer.transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator GenerateEnemies()
    {
        yield return new WaitForSeconds(2f); // espera X segundos durante la animaci�n
        foreach(var enemyData in enemiesData)
        {
            Vector2 position = enemyData.Key;
            EnemyType type = enemyData.Value;

            GameObject prefab = enemiesPrefabs[type];

            // Instanciamos enemigos normales
            GameObject enemy = Instantiate(
                prefab,
                position,
                Quaternion.identity,
                enemiesLayer.transform
            );

            if(type == EnemyType.Rushing)
            {
                enemy.GetComponent<RushingEnemyManager>().Init(player, gameManager);
            }
            else{
                enemy.GetComponent<Enemy>().Init(player,gameManager,audioManager);
            }
            

        }
    }
}
