using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = playerTransform.position + offset;
    }

    private void Update()
    {
        transform.position = playerTransform.position + offset;
    }
}
