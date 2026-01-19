using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    public float distance = 5f;
    float rotationY;
    float rotationX;
    public float minVerticalAngle = -45f;  //change in inspector mode--> for player rotation in up down
    public float maxVerticalAngle = 45f;




    private void Update()
    {
        rotationY += Input.GetAxis("Mouse X");
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle , maxVerticalAngle);
        rotationX += Input.GetAxis("Mouse Y");

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        transform.position = followTarget.position - targetRotation * new Vector3(0,0,distance);
        transform.rotation = targetRotation;
    }
}
