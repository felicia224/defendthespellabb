using UnityEngine;

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

    public void HandleButtonPress()
    {
        if (hasPressedButton) {
            return;
        }
        hasPressedButton = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, SphereRadius);

        GameObject nearestEnemy = hitColliders[0].gameObject;
        float closestDistance = Vector3.Distance(transform.position, hitColliders[0].transform.position);

        foreach (var collider in hitColliders) {
            if (!collider.CompareTag("Enemy")) {
                continue;
            }
            Debug.Log(collider.transform.name);
            float currentDistance = Vector3.Distance(transform.position, collider.transform.position);

            if (currentDistance < closestDistance) {
                nearestEnemy = collider.gameObject;
                closestDistance = currentDistance;
            }
            
        }
        
        Rigidbody rb = nearestEnemy.GetComponent<Rigidbody>();

        if (rb == null)
        {
            return;
        }

        rb.AddForce(Vector3.up * heightForce);
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
