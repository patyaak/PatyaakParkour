using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;      
    public float rotationSpeed = 200f; 

    CameraController cameraController;
    Quaternion targetRotation;

    private void Awake()
    {
      cameraController = Camera.main.GetComponent<CameraController>();
         
    }

    private void Update()
    {
       
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");

       var moveInput = (new Vector3(h,0,v)).normalized;
        var moveDir = cameraController.PlanerRotation * moveInput;

        if (moveInput.magnitude > 0)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);       
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
    }
}
