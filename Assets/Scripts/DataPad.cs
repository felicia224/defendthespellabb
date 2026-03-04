using UnityEngine;
using UnityEngine.UI;
public class DataPad : MonoBehaviour
{
    public Image screenImage;
    public Sprite startScreen;
    public Sprite storyScreen;
    public Sprite scoreScreen;

    public void ShowStart()
    {
        screenImage.sprite = startScreen;
    }

    public void ShowStory()
    {
        screenImage.sprite = storyScreen;
    }

    public void ShowScore()
    {
        screenImage.sprite= scoreScreen;
    }
}