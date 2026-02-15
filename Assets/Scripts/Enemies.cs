using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHealth = 100;

    private int currentHealth;
    private Transform player;
    private bool isActive = false;

    private EnemyQueueManager queueManager;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        queueManager = FindObjectOfType<EnemyQueueManager>();
    }

    void Update()
    {
        if (!isActive) return;

        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void ActivateEnemy()
    {
        isActive = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        queueManager.EnemyDied();
        Destroy(gameObject);
    }
}
