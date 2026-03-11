using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DataPad : MonoBehaviour
{
    public Image screenImage;
    public Sprite startScreen;
    public Sprite storyScreen;
    public Sprite scoreScreen;

    public TMP_Text scoreText;

    public void ShowStart()
    {
        screenImage.sprite = startScreen;

        if (scoreText != null)
            scoreText.text = "";
    }

    public void ShowStory()
    {
        screenImage.sprite = storyScreen;
        if (scoreText != null)
            scoreText.text = "";
    }

    public void ShowScore()
    {
        screenImage.sprite= scoreScreen;

        if (scoreText != null) {
            if (ScoreManager.instance != null)
            {
                scoreText.text = ScoreManager.instance.GetScore().ToString();
            }
        }
    }
}