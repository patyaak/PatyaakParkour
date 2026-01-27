using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;      
    public float rotationSpeed = 500f; 

    CameraController cameraController;
    Quaternion targetRotation;
    Animator animator;
    CharacterController characterController;

    private void Awake()
    {
      cameraController = Camera.main.GetComponent<CameraController>();
       animator = GetComponent<Animator>();
       characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
       
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis ("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

       var moveInput = (new Vector3(h,0,v)).normalized;
        var moveDir = cameraController.PlanerRotation * moveInput;

        if (moveAmount > 0)
        {
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
            
            targetRotation = Quaternion.LookRotation(moveDir);       
        }
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }
}
