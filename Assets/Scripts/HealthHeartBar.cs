using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthHeartBar : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;

    public Sprite emptyHeart;
    public Sprite fullHeart;

    public Image[] hearts;


    private void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite  = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;

            }
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
