using UnityEngine;
using UnityEngine.SceneManagement;

public class Slutmeny : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
