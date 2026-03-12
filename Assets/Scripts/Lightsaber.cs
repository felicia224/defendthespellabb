using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Lightsaber : MonoBehaviour
{
    //viewport to world eller tvärtom
    [SerializeField] private Camera cam;
    [SerializeField] private float distance;
    [SerializeField] private GameObject saber;
    private bool isActive = true;
    private Vector3 onScale = new Vector3(1, 1, 1);
    private Vector2 offScale = new Vector3(1, 0f, 1);
    [SerializeField] private float duration = 0.5f;
    private bool isEnemyLightsaber = false;

    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Cursor.visible = false;

        if (this.CompareTag("EnemyLightsaber"))
        {
            isEnemyLightsaber = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemyLightsaber)
        {
            //Vector3 screenMousePos = Mouse.current.position.ReadValue();
            //screenMousePos += Vector3.forward * distance;
            //Vector3 worldMousePos = cam.ScreenToWorldPoint(screenMousePos);

            //transform.position = worldMousePos;

            //transform.position = cam.transform.position + new Vector3(0, -1f, 2.5f);
            /*if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                float fingerY = touch.position.y;
                Vector3 pos = transform.position;
                pos.y = fingerY;
                transform.position = pos;

                Vector3 worldMousePos = cam.ScreenToWorldPoint(pos);

                transform.position = worldMousePos;
            }*/





            var keyboard = Keyboard.current;

            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                StartScaling();
            }

        }
    }

    private void StartScaling()
    {

        audioSource.Play();
        StartCoroutine(ScaleOverTime(duration));
    }

    private IEnumerator ScaleOverTime(float time)
    {

        Vector3 startScale;
        Vector3 targetScale;
        float elapsed = 0f;
        if (isActive)
        {
            startScale = onScale;
            targetScale = offScale;
            isActive= false;
        }
        else
        {
            startScale = offScale;
            targetScale = onScale;
            isActive = true;
        }
        Debug.Log("happens");
        saber.GetComponent<CapsuleCollider>().enabled = isActive;
        while (elapsed < time)
        {
            saber.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        


    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (isEnemyLightsaber)
        {
            Debug.Log("Triggered");
            if (other.gameObject.CompareTag("MainCamera"))
            {
                Debug.Log("Triggered camera!");
            }
        }
        
    }*/
}
