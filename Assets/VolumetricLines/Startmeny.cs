using UnityEngine;
using UnityEngine.SceneManagement;

public class Startmeny : MonoBehaviour
{
    public void StartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
