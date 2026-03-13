using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;

    public Sprite EmptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;


    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log("Player health: " + health);

        if (health == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Hantera spelarens död här
    }
}
