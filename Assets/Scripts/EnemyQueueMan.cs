/*
using UnityEngine;
using System.Collections.Generic;

public class EnemyQueueManager : MonoBehaviour
{
    private Queue<Enemy> enemyQueue = new Queue<Enemy>();

    void Start()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemyQueue.Enqueue(enemy);
        }

        ActivateNextEnemy();
    }

    public void EnemyDied()
    {
        ActivateNextEnemy();
    }

    void ActivateNextEnemy()
    {
        if (enemyQueue.Count > 0)
        {
            Enemy nextEnemy = enemyQueue.Dequeue();
            nextEnemy.ActivateEnemy();
        }
    }
}
*/