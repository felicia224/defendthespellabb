using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;

    private TextMeshPro textMesh;
    private Color textColor;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();

        if (textMesh == null)
        {
            Debug.LogError("No TextMeshPro found on DamagePopup!");
            return;
        }

        textColor = textMesh.color;
        timer = duration;
    }

    public void Setup(int damage)
    {
        if (textMesh != null)
        {
            textMesh.text = "-" + damage;
        }
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        Debug.Log("DamagePopup updating");

        if (textMesh != null)
        {
            textColor.a = Mathf.Clamp01(timer / duration);
            textMesh.color = textColor;
        }

        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}