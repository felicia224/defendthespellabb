using UnityEngine;

public class TheForce : MonoBehaviour
{
    [SerializeField] private RectTransform BarTransform;
    public int maxSize;
    private bool hasPressedButton;
    private float shrinkTime;
    [Range(0.0f, 1.0f)]
    public float barSpeed;

    public void HandleButtonPress()
    {
        if (hasPressedButton) {
            return;
        }
        hasPressedButton = true;
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
