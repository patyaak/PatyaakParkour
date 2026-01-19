using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [Header("Camera Settings")]
    public float distance = 5f;
    float rotationY;
    float rotationX;
    public float minVerticalAngle = -45f;  //change in inspector mode--> for player rotation in up down
    public float maxVerticalAngle = 45f;
    public Vector2 framingOffset;  //this is for how much the player can be seen
    private float rotationSpeed = 2f;

    [Header("Inverted Bools")]
    public bool invertX;
    public bool invertY;
    public float invertXVal;
    public float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;   
    }

    private void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Mouse Y")* invertYVal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle , maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X")* invertXVal * rotationSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPosition - targetRotation * new Vector3(0,0,distance);
        transform.rotation = targetRotation;
    }
}
