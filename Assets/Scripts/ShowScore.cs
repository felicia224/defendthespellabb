using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    void Start()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.scoreText = GetComponent<TMP_Text>();
        }
    }
}
