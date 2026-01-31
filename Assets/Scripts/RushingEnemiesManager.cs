using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushingEnemiesManager : MonoBehaviour
{   
    [SerializeField] MaskManager maskManager;
    [SerializeField] GameObject rushigEnemyPrefab;

    private List<Vector2> enemiesPositions;

    void Start()
    {
        GetEnemiesPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (!maskManager.canAct)
        {
            GenerateEnemies();
        }
        else
        {
            CheckEnemiesToReset();
        }
    }

    private void GetEnemiesPositions()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            enemiesPositions.Add(transform.GetChild(i).position);
        }
    }

    private void GenerateEnemies()
    {
        foreach(Vector2 pos in enemiesPositions)
        {
            Instantiate(rushigEnemyPrefab, pos, Quaternion.identity);
        }
    }

    private IEnumerator CheckEnemiesToReset()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Enemy enemy = transform.GetChild(i).GetComponent<Enemy>();
            if(enemy.GetHealth() <= 0)
            {   
                yield return new WaitForSeconds(3f);
                Destroy(enemy.gameObject);
                Instantiate(rushigEnemyPrefab, enemiesPositions[i], Quaternion.identity);
            }
        }
    }
}
