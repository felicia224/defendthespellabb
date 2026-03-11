using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;

    private TMP_Text damageText;
    private Color textColor;
    private float timer;

    void Awake()
    {
        damageText = GetComponentInChildren<TMP_Text>();
        if (damageText != null)
        {
            textColor = damageText.color;
        }

        timer = duration;
    }

    public void Setup(int damage)
    {
        if (damageText != null)
        {
            damageText.text = "-" + damage;
        }
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (damageText != null)
        {
            textColor.a = Mathf.Clamp01(timer / duration);
            damageText.color = textColor;
        }

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}