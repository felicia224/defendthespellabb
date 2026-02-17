using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyArenaManager manager;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    private bool moving = false;

    public bool IsAtPosition => !moving;

    public int maxHealth = 100; 
    private int currentHealth;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!moving) return;

        // Kolla om fienden n�tt sin destination
        if (!agent.pathPending && (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance))
        {
            StopMoving();
            manager.EnemyReachedPosition(this);
        }
    }

    void StopMoving()
    {
        moving = false;
        agent.enabled = false;
        obstacle.enabled = true; // Fienden blir solid och blockerar sloten
    }

    void StartMoving(Vector3 target)
    {
        obstacle.enabled = false;
        agent.enabled = true;

        moving = true;
        agent.SetDestination(target);
    }

    public void MoveTo(Transform pos)
    {
        StartMoving(pos.position);
    }

    public void EnterArena(Transform arenaPos)
    {
        StartMoving(arenaPos.position);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        manager.EnemyDied(this);
        Destroy(gameObject);
    }
}