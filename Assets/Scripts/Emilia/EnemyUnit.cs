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

    //private EnemyQueueManager enemyQM;

    [SerializeField] private Lightsaber enemyLightsaber;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    private float lastHitTime;
    public float hitCooldown = 0.2f;

    public GameObject damagePopupPrefab;
    [SerializeField] private Animator animator;

    private float timeBeforeAttack;
    [SerializeField] private const float interval = 3.0f;
    private bool readyToAttack;

    //Zoey la till:
    public PlayerHealth playerHealth;
    public float attackRange = 2.0f;
    public int attackDamage = 1; // Use 1 damage to match player health system

    //SLut på d z la till
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }


    void Start()
    {
        StartCoroutine(WaitForNavmesh());
        //enemyQM = FindAnyObjectByType<EnemyQueueManager>();
        agent = GetComponent<NavMeshAgent>();

        //Z
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
        //Z
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
        animator.SetTrigger("Hit");
        Debug.Log("Trooper health is: " +  currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowDamage(int damage)
    {
        if (damagePopupPrefab == null) return;

        Vector3 randomOffset = new Vector3(
            Random.Range(-0.25f, 0.25f),
            Random.Range(0f, 0.25f),
            Random.Range(-0.25f, 0.25f)
        );

        Debug.Log("ShowDamage spawn!");

        Vector3 spawnPos = transform.position + Vector3.up * 0.5f + randomOffset;

        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

        DamagePopup dp = popup.GetComponent<DamagePopup>();
        if (dp != null)
            dp.Setup(damage);
    }


    private void Die()
    {
        Debug.Log("dör");
        // Summera score
        if (ScoreManager.instance != null)
            ScoreManager.instance.AddScore(30);

        manager.OnEnemyDied(this);
  
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Lightsaber")){

            if (Time.time - lastHitTime < hitCooldown) return;
            lastHitTime = Time.time;

             // NYYYYYYYYYY
            TakeDamage(50);
        }*/

        if (other.CompareTag("AttackTrigger"))
        {
            readyToAttack = true;
            Debug.Log("Stormtroop entering attacktrigger");
        }
    }

    void Update()
    {
        if (readyToAttack)
        {
            WaitForAttack();
        }
        

        if (agent != null && animator != null)
        {
            // NavMeshAgent.velocity.magnitude är hastigheten
            float speed = agent.velocity.magnitude;

            // Sätt Speed i Animator
            animator.SetFloat("Speed", speed);
        }

        //Z
        if (playerHealth == null) return;

        float distance = Vector3.Distance(transform.position, playerHealth.transform.position);

        // Set readyToAttack based on player distance
        readyToAttack = distance <= attackRange;

        if (readyToAttack)
        {
            WaitForAttack();
        }
        else
        {
            timeBeforeAttack = 0f; // Reset attack timer if player is out of range
        }
        //Z end
    }

    private void AttackPlayer()
    {
        Debug.Log("Attacking");
        //här ska attackanimation läggas in nu
        animator.SetTrigger("AttackSword");

        //Z
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
        //Z

    }

    private void WaitForAttack()
    {
        timeBeforeAttack += Time.deltaTime;

        if (timeBeforeAttack >= interval)
        {
            timeBeforeAttack -= interval;

            AttackPlayer();
        }
    }

    public void KnockedBack()
    {
        Debug.Log("Knocked back");
        animator.SetTrigger("KnockBack");
    }

}