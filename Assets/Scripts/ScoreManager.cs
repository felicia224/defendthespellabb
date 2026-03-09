using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("In-game UI")]
    public TMP_Text scoreText;      // Live poäng i spel-scenen

    [Header("Menu UI")]
    public TMP_Text highScoreText;  // Highscore i menyn eller slutmenyn

    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Hämta highscore från tidigare spel
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
        UpdateHighScoreUI();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();

        // Kolla om highscore ska uppdateras
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("LastScore", score);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = "Highscore: " + highScore;
    }
}
