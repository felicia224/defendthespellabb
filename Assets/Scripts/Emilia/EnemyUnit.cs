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

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    private float lastHitTime;
    public float hitCooldown = 0.2f;

    public GameObject damagePopupPrefab;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
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

     public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ShowDamage(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ShowDamage(int damage)
{
    if (damagePopupPrefab == null) return;

    Vector3 randomOffset = new Vector3(
        Random.Range(-0.25f, 0.25f),
        Random.Range(0f, 0.25f),
        Random.Range(-0.25f, 0.25f)
    );

    Vector3 spawnPos = transform.position + Vector3.up * 2f + randomOffset;

    GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

    DamagePopup dp = popup.GetComponent<DamagePopup>();
    if (dp != null)
        dp.Setup(damage);
}


    void Die()
    {

        manager.OnEnemyDied(this);
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Lightsaber")) return;

        if (Time.time - lastHitTime < hitCooldown) return;
        lastHitTime = Time.time;

        TakeDamage(50);
    }
}