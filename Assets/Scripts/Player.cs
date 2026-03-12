using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int health;
    public HealthHeartBar healthHeartBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health = maxHealth; Debug.Log(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyLightsaber"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health--; Debug.Log(health);
        if(health <= 0)
        {
            KillPlayer();
        }

        if (healthHeartBar != null)
        {
            healthHeartBar.health = health;  // update heart bar health
        }
    }

    public void KillPlayer()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }
}
