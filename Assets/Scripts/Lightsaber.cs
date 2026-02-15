using UnityEngine;
using UnityEngine.InputSystem;

public class Lightsaber : MonoBehaviour
{
    //viewport to world eller tvärtom
    [SerializeField] private Camera cam;
    [SerializeField] private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenMousePos = Mouse.current.position.ReadValue();
        screenMousePos += Vector3.forward*distance;
        Vector3 worldMousePos = cam.ScreenToWorldPoint(screenMousePos);

        transform.position = worldMousePos;
    }
}
