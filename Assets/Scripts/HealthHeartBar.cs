using UnityEngine;
using UnityEngine.UI;

public class HealthHeartBar : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Settings")]
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image[] hearts;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }

    /// <summary>
    /// Call this method to reduce health by damage amount
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player health: " + currentHealth);

        UpdateHeartsUI();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Updates the heart UI based on current health and max health
    /// </summary>
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hearts[i].enabled = i < maxHealth;
        }
    }

    /// <summary>
    /// Handles player death
    /// </summary>
    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Add death handling here (reload scene, show UI, etc.)
        // Example: UnityEngine.SceneManagement.SceneManager.LoadScene(yourDeathSceneIndex);
    }

    /// <summary>
    /// Optional: Method to heal player
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartsUI();
    }

    /// <summary>
    /// Optional: Get current health value
    /// </summary>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
