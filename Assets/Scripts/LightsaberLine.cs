using UnityEngine;

public class LightsaberLine : MonoBehaviour
{

    [SerializeField] private bool isEnemyLightsaber = false;
    [SerializeField] private EnemyUnit enemyHolder = null;

    private bool playerCanTakeDamage;
    private float timeBeforeEnemyAttack;
    [SerializeField] private const float interval = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (this.CompareTag("EnemyLightsaber"))
        {
            isEnemyLightsaber = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCanTakeDamage)
        {
            WaitForEnemyAttack();
        }
    }
    private void WaitForEnemyAttack()
    {
        timeBeforeEnemyAttack += Time.deltaTime;

        if (timeBeforeEnemyAttack >= interval)
        {
            timeBeforeEnemyAttack -= interval;

            playerCanTakeDamage = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isEnemyLightsaber)
        {
            EnemyUnit enemy = other.gameObject.GetComponent<EnemyUnit>();
            enemy.TakeDamage(50);
        }

        if (other.gameObject.CompareTag("Lightsaber") && isEnemyLightsaber)
        {
            Debug.Log("saber hit saber");
            enemyHolder.KnockedBack();
            
        }

        //Z
        if (other.CompareTag("MainCamera") && isEnemyLightsaber)
        {
            Debug.Log("Player hit!");

            playerCanTakeDamage = false;

            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage();
            }
        }
        //z
    }
}
