using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyUnit : MonoBehaviour
{
    public EnemyQueueManager manager;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    private Vector3 lastDest;
    private bool hasLastDest = false;

    private bool ready = false;
    private Transform pendingTarget;

    private Coroutine arriveRoutine;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        if (obstacle != null)
            obstacle.enabled = false; // inte solid när den rör sig
    }

    void Start()
    {
        StartCoroutine(WaitForNavmesh());
    }


    IEnumerator WaitForNavmesh()
{
    if (!agent.isOnNavMesh)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }
    Debug.Log($"[{name}] READY. isOnNavMesh={agent.isOnNavMesh}");

    while (!agent.isOnNavMesh)
        yield return null;

    ready = true;
    manager.NotifyUnitReady();

    if (pendingTarget != null)
        MoveTo(pendingTarget);
}

    public void MoveTo(Transform target)
    {
        if (!ready)
        {
            pendingTarget = target;
            return;
        }

        pendingTarget = null;

        // Gör den "ghost" och starta agent
        if (obstacle != null) obstacle.enabled = false;
        if (!agent.enabled) agent.enabled = true;

        agent.isStopped = false;
        Debug.Log($"[{name}] MoveTo -> {target.name} pos={target.position}");

        agent.SetDestination(target.position);

        Vector3 dest = target.position;
    NavMeshHit hit;

    if (NavMesh.SamplePosition(dest, out hit, 2f, NavMesh.AllAreas))
    {
        dest = hit.position;
    }

    Debug.Log($"[{name}] SetDestination -> {dest}");
    if (hasLastDest && Vector3.Distance(lastDest, dest) < 0.05f)
    {
    // samma destination, gör inget
        return;
    }

    hasLastDest = true;
    lastDest = dest;


    agent.SetDestination(dest);

    if (arriveRoutine != null)
        StopCoroutine(arriveRoutine);

    arriveRoutine = StartCoroutine(ArriveCheck());
    }

    IEnumerator ArriveCheck()
{
    // Vänta tills Unity börjar räkna path
    float timer = 0f;
    while (agent.pathPending && timer < 1f)
    {
        timer += Time.deltaTime;
        yield return null;
    }

    // Vänta lite på att en path ska dyka upp (agent.hasPath kan vara false i början)
    timer = 0f;
    while (!agent.hasPath && timer < 1f)
    {
        // Om destinationen är omöjlig (t.ex. utanför navmesh) så avbryt snyggt
        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogWarning($"[{name}] PathInvalid -> kan inte nå destination.");
            yield break; // lämna agenten igång så vi kan testa igen senare
        }

        timer += Time.deltaTime;
        yield return null;
    }

    if (!agent.hasPath)
    {
        Debug.LogWarning($"[{name}] Ingen path hittades (än).");
        yield break;
    }

    // Nu: vänta tills vi är framme
    while (agent.remainingDistance > agent.stoppingDistance)
        yield return null;

    // Lås i slot: stoppa agent, slå på obstacle
    agent.isStopped = true;
    agent.ResetPath();
    agent.enabled = false;

    if (obstacle != null)
        obstacle.enabled = true;
}

}
