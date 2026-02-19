using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float lifetime = 1f;

    private TextMeshPro text;
    private Color color;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        color = text.color;
    }

    public void Setup(int damage)
    {
        text.text = "-" + damage;
    }

    void Update()
    {
        // Flyt uppåt
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade
        lifetime -= Time.deltaTime;
        color.a = lifetime;
        text.color = color;

        if (lifetime <= 0)
            Destroy(gameObject);
    }
}
