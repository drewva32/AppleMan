using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoxCollider bounds;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private new Camera camera;
    private float leftBound;
    private float rightBound;
    private float upBound;
    private float downBound;

    private void Start()
    {
        camera = GetComponent<Camera>();

        leftBound = bounds.transform.position.x - (bounds.size.x * 0.5f);
        rightBound = bounds.transform.position.x + (bounds.size.x * 0.5f);
        upBound = bounds.transform.position.y + (bounds.size.y * 0.5f);
        downBound = bounds.transform.position.y - (bounds.size.y * 0.5f);
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        float height = camera.orthographicSize;
        float width = height * camera.aspect;

        if (desiredPosition.x < leftBound + width)
        {
            desiredPosition.x = leftBound + width;
        }

        if (desiredPosition.x > rightBound - width)
        {
            desiredPosition.x = rightBound - width;
        }

        if (desiredPosition.y > upBound - height)
        {
            desiredPosition.y = upBound - height;
        }

        if (desiredPosition.y < downBound + height)
        {
            desiredPosition.y = downBound + height;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // transform.LookAt(target);
    }

}
