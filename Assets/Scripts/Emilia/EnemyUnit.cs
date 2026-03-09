using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class EnemyUnit : MonoBehaviour
{
    public EnemyQueueManager manager;
    public TMP_Text scoreText;

    public TheForce theForceScript;

    private NavMeshAgent agent;

    private bool ready = false;
    private Transform pendingTarget;

    private EnemyQueueManager enemyQM;

    [SerializeField] private Lightsaber enemyLightsaber;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    private float lastHitTime;
    public float hitCooldown = 0.2f;

    public GameObject damagePopupPrefab;
    private Animator animator;

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
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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

    Debug.Log("ShowDamage spawn!");

    Vector3 spawnPos = transform.position + Vector3.up * 2.5f + randomOffset;

    GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

    DamagePopup dp = popup.GetComponent<DamagePopup>();
    if (dp != null)
        dp.Setup(damage);
}


    void Die()
    {

        // Summera score
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(30);

        manager.OnEnemyDied(this);
  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Lightsaber")) return;

        if (Time.time - lastHitTime < hitCooldown) return;
        lastHitTime = Time.time;

        TakeDamage(50);
    }

    void Update()
    {
        if (agent != null && animator != null)
        {
            // NavMeshAgent.velocity.magnitude är hastigheten
            float speed = agent.velocity.magnitude;

            // Sätt Speed i Animator
            animator.SetFloat("Speed", speed);
        }
    }

    private void AttackPlayer()
    {
        
    }
}