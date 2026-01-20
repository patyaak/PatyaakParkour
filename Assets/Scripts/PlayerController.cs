using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reduced speeds based on user feedback
    public float moveSpeed = 3f;      // Was 5
    public float rotationSpeed = 200f; // Was 700 (User set), now 200 for smoother turns

    CameraController cameraController;
    Quaternion targetRotation;

    private void Awake()
    {
        // Safety checks to prevent "MissingReferenceException" and Editor crashes
        if (Camera.main != null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }
        else
        {
            Debug.LogError("Main Camera is missing! Tag your camera as 'MainCamera'.");
        }

        if (cameraController == null)
        {
            Debug.LogError("CameraController script is missing from Main Camera!");
        }
    }

    private void Update()
    {
        if (cameraController == null) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");

        // Clamp magnitude prevents diagonal speed boost while allowing slow acceleration
        var moveInput = Vector3.ClampMagnitude(new Vector3(h, 0, v), 1f);
        var moveDir = cameraController.PlanerRotation * moveInput;

        if (moveInput.magnitude > 0)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);       
        }
        
        // RotateTowards allows smooth rotation over time
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
    }
}
