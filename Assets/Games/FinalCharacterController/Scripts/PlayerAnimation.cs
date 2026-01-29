using UnityEngine;

namespace PatyaakGame.FinalCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private PlayerLocomotionInput playerLocomotionInput;
        private PlayerState playerState;

        [SerializeField] private float locomotionBlendSpeed = 5f;
        

        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");
        private Vector3 currentBlendInput = Vector3.zero;


        private void Awake()
        {
            playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            Vector2 inputTarget = playerLocomotionInput.MovementInput;
            currentBlendInput = Vector3.Lerp(currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);


            animator.SetFloat(inputXHash, currentBlendInput.x);
            animator.SetFloat (inputYHash, currentBlendInput.y);
        }
    }

}


