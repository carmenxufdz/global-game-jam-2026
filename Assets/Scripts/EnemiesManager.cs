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
    [SerializeField] MaskManager maskManager;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject player;

    [Header("Prefabs")]
    [SerializeField] GameObject walkingEnemyPrefab;
    [SerializeField] GameObject FlyingEnemyPrefab;
    [SerializeField] GameObject rushingEnemyPrefab;

    private int totalEnemies;
    private Dictionary<Vector2,EnemyType> enemiesData = new();
    private Dictionary<EnemyType, GameObject> enemiesPrefabs;

    void Start()
    {   
        enemiesPrefabs = new Dictionary<EnemyType, GameObject>
        {
            {EnemyType.Walking, walkingEnemyPrefab},
            {EnemyType.Flying, FlyingEnemyPrefab},
            {EnemyType.Rushing, rushingEnemyPrefab}
        };

        GetEnemiesData(); //Guardamos posicio y tipo de cada enemigo para generarlos después
    }

    void Update()
    {
        //Si se está cambiando el mundo
        if (!maskManager.canAct)
        {
            DestroyEnemies(); //Eliminamos todos los enemigos
            GenerateEnemies(); //Los generamos de nuevo en sus posiciones originales
        }
    }

    private void GetEnemiesData()
    {
        totalEnemies = enemiesLayer.transform.childCount;
        for(int i = 0; i < enemiesLayer.transform.childCount; i++)
        {
            GameObject enemy = enemiesLayer.transform.GetChild(i).gameObject;
            enemiesData.Add(enemy.transform.position,enemy.GetComponent<Enemy>().GetEnemyType());
            print("Enemigo guardado");
        }
    }

    private void DestroyEnemies()
    {
        for(int i = 0; i < totalEnemies ; i++)
        {
            Destroy(enemiesLayer.transform.GetChild(i).gameObject);
            print("Enemigo destruido");
        }
    }

    private void GenerateEnemies()
    {
        foreach(var enemyData in enemiesData)
        {
            Vector2 position = enemyData.Key;
            EnemyType type = enemyData.Value;

            GameObject prefab = enemiesPrefabs[type];
            GameObject enemy = Instantiate(
                prefab,
                position,
                Quaternion.identity,
                enemiesLayer.transform
            );

            enemy.GetComponent<Enemy>().Init(player,gameManager);
            print("Enemigo generado");

        }
    }
}
