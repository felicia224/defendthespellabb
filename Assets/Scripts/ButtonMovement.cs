using UnityEngine;

public class ButtonIdleMove : MonoBehaviour
{
    public float moveAmount = 5f;   // Hur långt den rör sig upp/ner
    public float moveSpeed = 2f;    // Hur snabbt den rör sig

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
        transform.localPosition = startPos + new Vector3(0, offset, 0);
    }
}