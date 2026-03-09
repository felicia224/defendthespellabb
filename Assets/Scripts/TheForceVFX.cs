using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ForceLightningVFX : MonoBehaviour
{
    public int segments = 20;
    public float amplitude = 0.25f;
    public float noiseSpeed = 25f;

    public float lifetime = 0.2f;

    public float widthFlicker = 0.03f;

    private LineRenderer lr;
    private Transform startPoint;
    private Transform endPoint;

    private float timer;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Play(Transform from, Transform to)
    {
        startPoint = from;
        endPoint = to;

        lr.positionCount = segments;
        timer = lifetime;
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        timer -= Time.deltaTime;

        DrawLightning();

        FlickerWidth();

        if (timer <= 0)
            Destroy(gameObject);
    }

    void DrawLightning()
    {
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            Vector3 pos = Vector3.Lerp(start, end, t);

            float noiseX = (Mathf.PerlinNoise(t * 4f, Time.time * noiseSpeed) - 0.5f) * amplitude;
            float noiseY = (Mathf.PerlinNoise(t * 4f + 50f, Time.time * noiseSpeed) - 0.5f) * amplitude;

            pos += new Vector3(noiseX, noiseY, 0);

            lr.SetPosition(i, pos);
        }
    }

    void FlickerWidth()
    {
        float flicker = Random.Range(0.01f, widthFlicker);

        lr.startWidth = flicker;
        lr.endWidth = flicker * 0.3f;
    }
}