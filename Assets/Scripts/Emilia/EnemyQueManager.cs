using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyQueueManager : MonoBehaviour
{
    [Header("Points")]
    public Transform spawnPoint;
    public Transform[] queuePoints; // 5 st
    public Transform attackPoint;

    [Header("Wave")]
    public EnemyWaveData[] waves;
    private EnemyWaveData wave;
    public float spawnDelay = 1f;
    private int currentWaveIndex = 0;

    [Header("Gameplay")]
    public bool battleStarted = false;

    private List<EnemyUnit> slots = new List<EnemyUnit>(); // 7 slots
    private int spawnIndex = 0;
    private bool spawning = false;
    private bool spawnBusy = false;
    private bool gameFinished = false;

    public int killScore;

    [SerializeField] GameObject forceButton;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject killButton;
    [SerializeField] GameObject killmeButton;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text waveText;

    void Start()
    {
        int totalSlots = queuePoints.Length + 1; // + attackpoint
        for (int i = 0; i < totalSlots; i++)
            slots.Add(null);

        waveText.gameObject.SetActive(false);

        

        forceButton.SetActive(false);
        startButton.SetActive(true);
        killButton.SetActive(false);
        killmeButton.SetActive(false);
    }

    public void StartBattle()
    {
        battleStarted = true;
        forceButton.SetActive(true);
        startButton.SetActive(false) ;
        killButton.SetActive(true);
        killmeButton.SetActive(true);

        StartNextWave();
        StartCoroutine(ShowWaveText(currentWaveIndex + 1));

        UpdateAllPositions(); // frontfienden b�rjar g� mot attack
    }

    void StartNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
            return;
        }

        spawnIndex = 0;
        wave = waves[currentWaveIndex];

        Debug.Log("Starting wave: " + (currentWaveIndex + 1));

        if (battleStarted)
        {
            StartCoroutine(ShowWaveText(currentWaveIndex + 1));
        }

        StartCoroutine(SpawnRoutine());
    }

    public void KillMe()
    {
        SceneManager.LoadScene(2);
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
                yield return null; 
            }
        }

        spawning = false;

        StartCoroutine(CheckWaveFinished());
    }

    IEnumerator CheckWaveFinished()
    {
        while (true)
        {
            bool enemiesLeft = false;

            foreach (EnemyUnit unit in slots)
            {
                if (unit != null)
                {
                    enemiesLeft = true;
                    break;
                }
            }

            if (!enemiesLeft)
            {
                currentWaveIndex++;

                if (currentWaveIndex >= waves.Length)
                {
                    // Sista wave klar → Victory
                    if (!gameFinished)
                    {
                        gameFinished = true;
                        StartCoroutine(LoadVictoryScene());
                    }
                    yield break;
                }
                else
                {
                    // Nästa wave
                    yield return new WaitForSeconds(2f);
                    StartNextWave();
                    yield break;
                }
            }

            yield return null;
        }
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
        killScore++;
        scoreText.text = "Score: " + killScore;

        ShiftForward();
    }

    public void OnEnemyDied(EnemyUnit unit)
{
    int index = slots.IndexOf(unit);
    if (index < 0) return;

    slots[index] = null;

    Destroy(unit.gameObject);

    if (index == 0)
    {
        ShiftForward();
    }
    else
    {
        UpdateAllPositions();
    }
}


    void ShiftForward()
    {
        for (int i = 0; i < slots.Count - 1; i++)
            slots[i] = slots[i + 1];

        slots[slots.Count - 1] = null;

        // Uppdatera alla targets baserat p� nya slots
        UpdateAllPositions();
    }

    void UpdateAllPositions()
    {
        Debug.Log("UpdateAllPositions()");
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;

            Transform target;

            if (i == 0 && battleStarted)
            {
                target = attackPoint; // fronten g�r till attackPoint f�rst n�r battle startar
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
                    pointIndex = queuePoints.Length - 1 - i; // kvar p� k�
                }

                if (pointIndex < 0) pointIndex = 0;
                target = queuePoints[pointIndex];
            }

            slots[i].MoveTo(target);
        }
    }

    bool CanSpawnNext()
    {
        // Kolla bara k�platser (inte fronten)
        for (int i = 1; i < queuePoints.Length + 1; i++)
        {
            if (slots[i] == null)
                return true; // finns plats i k�n
        }
        return false;
    }

    public void NotifyUnitReady()
    {
        Debug.Log("Unit ready -> updating positions");
        UpdateAllPositions();
    }

    IEnumerator ShowWaveText(int waveNumber)
    {
        waveText.gameObject.SetActive(true);
        
        if(currentWaveIndex == waves.Length - 1)
        {
            waveText.text = "Final Wave!";
        }else
        {
            waveText.text = "Wave " + waveNumber + " incoming!";
        }

        yield return new WaitForSeconds(3f);

        waveText.gameObject.SetActive(false);
    }

    IEnumerator LoadVictoryScene()
    {
        waveText.gameObject.SetActive(true);
        waveText.text = "victory!";
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(2);
    }
}