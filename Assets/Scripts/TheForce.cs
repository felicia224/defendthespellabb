using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TheForce : MonoBehaviour
{
    [SerializeField] private RectTransform BarTransform;
    public int maxSize;
    private bool hasPressedButton;
    private float shrinkTime;
    [Range(0.0f, 1.0f)]
    public float barSpeed;
    public float SphereRadius;
    public float heightForce;

    public Transform attackPoint;

    public void HandleButtonPress()
{
    if (hasPressedButton) return;
    hasPressedButton = true;

    Collider[] hitColliders = Physics.OverlapSphere(transform.position, SphereRadius);
    if (hitColliders.Length == 0) return;

    GameObject nearestEnemy = null;
    float closestDistance = Mathf.Infinity;

    foreach (var col in hitColliders)
    {
        if (!col.CompareTag("Enemy")) continue;

        float d = Vector3.Distance(transform.position, col.transform.position);
        if (d < closestDistance)
        {
            closestDistance = d;
            nearestEnemy = col.gameObject;
        }
    }

    if (nearestEnemy == null) return;

    EnemyUnit unit = nearestEnemy.GetComponent<EnemyUnit>();
    if (unit != null)
    {
        unit.TakeDamage(50); 
        if (unit.currentHealth <= 0) return; 
    }

    StartCoroutine(forcePush(nearestEnemy));
}


    private IEnumerator forcePush(GameObject nearestEnemy) {

        Rigidbody rb = nearestEnemy.GetComponent<Rigidbody>();

        if (rb == null) {
            yield return null;
        }

        NavMeshAgent agent = nearestEnemy.GetComponent<NavMeshAgent>();
        EnemyUnit unit = nearestEnemy.GetComponent<EnemyUnit>();
        agent.enabled = false;
        unit.enabled = false;
        //yield return new WaitForSeconds(0.5f);

        rb.AddForce(Vector3.up * heightForce);

        yield return new WaitForSeconds(3);

        agent.enabled = true;
        unit.enabled = true;

        yield return new WaitUntil(() => agent.isOnNavMesh);

        agent.SetDestination(attackPoint.position);
        //agent.ResetPath();
    }


    public void Update()
    {

        if (hasPressedButton) {
            BarTransform.sizeDelta = new Vector2(Mathf.Lerp(maxSize, 0, shrinkTime),100);
            shrinkTime -= barSpeed * Time.deltaTime;

            if (BarTransform.sizeDelta.x == maxSize)
            {
                hasPressedButton = false;
                shrinkTime = 1.0f;
            }
        }
    }

    public void Start()
    {
        hasPressedButton = false;
        shrinkTime = 1.0f;
    }
}
