using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyQueueManager : MonoBehaviour
{
    [Header("Points")]
    public Transform spawnPoint;
    public Transform[] queuePoints; // 5 st
    public Transform attackPoint;

    [Header("Wave")]
    public EnemyWaveData wave;
    public float spawnDelay = 1f;

    [Header("Gameplay")]
    public bool battleStarted = false;

    private List<EnemyUnit> slots = new List<EnemyUnit>(); // 7 slots
    private int spawnIndex = 0;
    private bool spawning = false;
    private bool spawnBusy = false;

    void Start()
    {
        int totalSlots = queuePoints.Length + 1; // + attackpoint
        for (int i = 0; i < totalSlots; i++)
            slots.Add(null);

        StartCoroutine(SpawnRoutine());
    }

    public void StartBattle()
    {
        battleStarted = true;

        UpdateAllPositions(); // frontfienden börjar gå mot attack
    }

    IEnumerator SpawnRoutine()
    {
        spawning = true;

        while (spawnIndex < wave.enemiesInOrder.Length)
        {
            if (!spawnBusy && CanSpawnNext())
            {
                SpawnEnemy(wave.enemiesInOrder[spawnIndex]);
                spawnIndex++;
                yield return new WaitForSeconds(spawnDelay);
            }
            else
            {
                yield return null; // vänta tills plats finns
            }
        }

        spawning = false;
    }

    void SpawnEnemy(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        EnemyUnit unit = go.GetComponent<EnemyUnit>();

        unit.manager = this;

        spawnBusy = true;

        InsertIntoQueue(unit);
    }

    bool InsertIntoQueue(EnemyUnit unit)
{
    for (int i = 0; i < slots.Count; i++)
    {
        if (slots[i] == null)
        {
            slots[i] = unit;
            spawnBusy = false;
            return true;
        }
    }
    return false; // ingen plats
}

    public void KillFrontEnemy()
    {
        if (slots[0] == null) return;

        Destroy(slots[0].gameObject);
        ShiftForward();
    }

    void ShiftForward()
    {
        for (int i = 0; i < slots.Count - 1; i++)
            slots[i] = slots[i + 1];

        slots[slots.Count - 1] = null;

        // Uppdatera alla targets baserat på nya slots
        UpdateAllPositions();
    }

    void UpdateAllPositions()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;

            Transform target;

            if (i == 0 && battleStarted)
            {
                target = attackPoint; // fronten går till attackPoint först när battle startar
            }
            else
            {
                int pointIndex;
                if (battleStarted)
                {
                    pointIndex = queuePoints.Length - i; // flytta fram ett steg
                }
                else
                {
                    pointIndex = queuePoints.Length - 1 - i; // kvar på kö
                }

                if (pointIndex < 0) pointIndex = 0;
                target = queuePoints[pointIndex];
            }

            slots[i].MoveTo(target);
        }
    }

    bool CanSpawnNext()
    {
        // Kolla bara köplatser (inte fronten)
        for (int i = 1; i < queuePoints.Length + 1; i++)
        {
            if (slots[i] == null)
                return true; // finns plats i kön
        }
        return false;
    }

    public void NotifyUnitReady()
    {
        UpdateAllPositions();
    }
}