using UnityEngine;
using UnityEngine.AI;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyUnit : MonoBehaviour
{
    public EnemyQueueManager manager;
    private NavMeshAgent agent;

    private bool ready = false;
    private Transform pendingTarget;

    private EnemyQueueManager enemyQM;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        StartCoroutine(WaitForNavmesh());
        enemyQM = FindAnyObjectByType<EnemyQueueManager>();
    }

    IEnumerator WaitForNavmesh()
    {
        while (!agent.isOnNavMesh)
            yield return null;

        ready = true;

        manager.NotifyUnitReady();

        if (pendingTarget != null)
            agent.SetDestination(pendingTarget.position);
    }

    public void MoveTo(Transform target)
    {
        if (!ready || !agent.isOnNavMesh)
        {
            pendingTarget = target;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Lightsaber")
        {
            enemyQM.KillFrontEnemy();
        }
    }
}