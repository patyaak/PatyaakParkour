using UnityEngine;

namespace PatyaakGame.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
  
        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;


        [Header("Base Movement")]
        public float runAcceleration = 35f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;
        public float drag = 20f;
        public float movingThreshold = 0.01f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerLocomotionInput playerLocomotionInput;
        private PlayerState playerState;
        private Vector2 cameraRotation = Vector2.zero;
        private Vector2 playerTargetRotation = Vector2.zero;

  
        private void Awake()
        {
            playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateMovementState();
            HandleLateralMovement();
        }

        private void UpdateMovementState()
        {
            bool isMovementInput = playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = playerLocomotionInput.SprintToggledOn && isMovingLaterally;

            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                isMovingLaterally || isMovementInput? PlayerMovementState.Running:PlayerMovementState.Idling;
            playerState.SetPlayerMovementState(lateralState);

        }
        private void HandleLateralMovement()
        {
            bool isSprinting = playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;

            float lateralAcceleration = isSprinting ? sprintAcceleration: runAcceleration;
            float clampAcceleration = isSprinting ? sprintSpeed : runSpeed;

            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * playerLocomotionInput.MovementInput.x + cameraForwardXZ * playerLocomotionInput.MovementInput.y;

            Vector3 movementDelta = movementDirection * lateralAcceleration;
            Vector3 newVelocity = characterController.velocity + movementDelta;

            // Add drag to player
            Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(newVelocity, clampAcceleration);

            // Move character (Unity suggests only calling this once per tick)
            characterController.Move(newVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            cameraRotation.x += lookSenseH * playerLocomotionInput.LookInput.x;
            cameraRotation.y = Mathf.Clamp(cameraRotation.y - lookSenseV * playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);

            playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
        }

        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.y);
            return lateralVelocity.magnitude > movingThreshold;
        }
    }
}

