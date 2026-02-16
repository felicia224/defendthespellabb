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

    IEnumerator SpawnRoutine()
    {
        spawning = true;

        while (spawnIndex < wave.enemiesInOrder.Length)
        {
            if (!spawnBusy)
            {
                SpawnEnemy(wave.enemiesInOrder[spawnIndex]);
                spawnIndex++;
                yield return new WaitForSeconds(spawnDelay);
            }
            else
            {
                yield return null;
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

    void InsertIntoQueue(EnemyUnit unit)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = unit;
                UpdateAllPositions();
                spawnBusy = false;
                return;
            }
        }
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
        {
            slots[i] = slots[i + 1];
        }

        slots[slots.Count - 1] = null;

        UpdateAllPositions();
    }

    void UpdateAllPositions()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;

            Transform target = GetPoint(i);
            slots[i].MoveTo(target);
        }
    }

    Transform GetPoint(int index)
    {
        // index 0 = front (attackPoint)
        if (index == 0)
            return attackPoint;

        // resten = queuePoints, från queuePoints[queuePoints.Length - 1] bakåt
        int queueIndex = index - 1;
        if (queueIndex < queuePoints.Length)
            return queuePoints[queueIndex];

        return queuePoints[queuePoints.Length - 1]; // fallback
    }

    public void NotifyUnitReady()
    {
        UpdateAllPositions();
    }
}