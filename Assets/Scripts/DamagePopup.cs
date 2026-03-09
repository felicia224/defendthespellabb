using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;

    private TMP_Text text;
    private Color color;
    private float timer;

    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        if (text != null)
            color = text.color;

        timer = duration;
    }

    public void Setup(int damage)
    {
        if (text != null)
            text.text = "-" + damage;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (text != null)
        {
            color.a = Mathf.Clamp01(timer / duration);
            text.color = color;
        }

        if (timer <= 0f)
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}
