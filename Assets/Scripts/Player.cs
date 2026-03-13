using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //z

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int health;
    public HealthHeartBar healthHeartBar;

    //z
    [Header("Heart UI")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    //Z

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health = maxHealth; Debug.Log(health);

        UpdateHearts();//z

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

    public void TakeDamage()
    {
        health--; Debug.Log(health);
        if(health <= 0)
        {
            KillPlayer();
        }
        //Z
        UpdateHearts();
        if(health<= 0)
        {
            KillPlayer();
        }
        /*
        if (healthHeartBar != null)
        {
          healthHeartBar.health = health;  // update heart bar health
        }
        */
    }

    //Z
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
    //Z

    public void KillPlayer()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }
}
