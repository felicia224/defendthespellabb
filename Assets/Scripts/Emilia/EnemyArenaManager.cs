using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArenaManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] queuePositions;
    public Transform fightPosition;

    private Queue<EnemyAI> spawnQueue = new Queue<EnemyAI>();
    private EnemyAI[] slots;
    private EnemyAI currentFighter;

    void Start()
    {
        slots = new EnemyAI[queuePositions.Length];
        StartCoroutine(SpawnRoutine(5)); // Spawn 5 fiender som exempel
    }

    IEnumerator SpawnRoutine(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyAI enemy = obj.GetComponent<EnemyAI>();
            enemy.manager = this;

            spawnQueue.Enqueue(enemy);
            TryMoveForward();

            // Vänta tills sista slot i kön är ledig innan nästa spawn
            while (spawnQueue.Count > 0 && slots[slots.Length - 1] != null)
            {
                yield return null;
            }
        }
    }

    void TryMoveForward()
    {
        // Flytta fram endast en fiende i taget
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null && spawnQueue.Count > 0)
            {
                EnemyAI next = spawnQueue.Dequeue();
                slots[i] = next;
                next.MoveTo(queuePositions[i]);
                break; // bara en fiende åt gången
            }
        }

        // Om fight-position är ledig, skicka första fienden
        if (currentFighter == null && slots[0] != null && slots[0].IsAtPosition)
        {
            SendFirstToFight();
        }
    }

    public void EnemyReachedPosition(EnemyAI enemy)
    {
        TryMoveForward();

        if (slots[0] == enemy && currentFighter == null)
        {
            SendFirstToFight();
        }
    }

    void SendFirstToFight()
    {
        currentFighter = slots[0];
        slots[0] = null;

        currentFighter.EnterArena(fightPosition);
        ShiftQueue();
    }

    void ShiftQueue()
    {
        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i - 1] == null)
            {
                slots[i - 1] = slots[i];
                slots[i] = null;
                slots[i - 1].MoveTo(queuePositions[i - 1]);
            }
        }

        TryMoveForward();
    }

    public void EnemyDied(EnemyAI enemy)
    {
        if (enemy == currentFighter)
        {
            currentFighter = null;
            Invoke(nameof(TryMoveForward), 1f); // Vänta lite innan nästa fiende går in
        }
    }
}