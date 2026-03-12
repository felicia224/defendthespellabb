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
        textColor = textMesh.color;
        timer = duration;
    }

    public void Setup(int damage)
    {
        textMesh.text = "-" + damage;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        textColor.a = Mathf.Clamp01(timer / duration);
        textMesh.color = textColor;

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