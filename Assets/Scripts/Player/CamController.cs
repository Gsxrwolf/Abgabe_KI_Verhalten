using System;
using UnityEngine;

public class CamController : MonoBehaviour
{

    [SerializeField] private string wolfTag = "Wolf";

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float speed;

    private Camera cam;

    public static event Action<Vector3> newDogDestination;
    public static event Action<GameObject> followWolf;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float distanceMultiplyer = ((transform.position.y / 80) - 0.3f) * 3.33f;
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed * distanceMultiplyer;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * distanceMultiplyer;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * speed * distanceMultiplyer;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * distanceMultiplyer;
        }
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Vector3 movement = Input.mouseScrollDelta.y * transform.forward;
            Vector3 newPos = transform.position + movement;
            if (newPos.y > minDistance && newPos.y < maxDistance)
                transform.position = newPos;
            else
                transform.position = new Vector3(newPos.x, Mathf.Clamp(newPos.y, minDistance, maxDistance), transform.position.z);

        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag(wolfTag))
                {
                    followWolf?.Invoke(hit.collider.gameObject);
                }
                else
                {
                    newDogDestination?.Invoke(hit.point);
                }
            }

        }
    }
}
