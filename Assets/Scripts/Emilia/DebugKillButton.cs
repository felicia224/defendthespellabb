using UnityEngine;

public class DebugKillButton : MonoBehaviour
{
    public EnemyQueueManager manager;

    public void KillEnemy()
    {
        manager.KillFrontEnemy();
    }
}
