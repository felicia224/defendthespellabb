using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKillButton : MonoBehaviour
{
    public EnemyQueueManager manager;
    [SerializeField] GameObject killmeButton;

    public void KillEnemy()
    {
        manager.KillFrontEnemy();
    }

    public void KillMe()
    {
        SceneManager.LoadScene(2);
    }
}
