using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Sprint speed of the character in m/s")]
        public float MoveSpeed = 5.335f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Audio")]
        [SerializeField]
        private AudioSource footstepAudio;

        private bool isWalking = false;

        // player
        private float _targetRotation;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _fallTimeoutDelta;

        [SerializeField]
        private Animator animator;

        private CharacterController _controller;
        private StarterAssetsInputs _input;


        [SerializeField]
        private float movementThreshold = 0.002f;

        [SerializeField]
        private Vector3 horizontalVelocity;

        private int _xAnimatorParam;
        private int _zAnimatorParam;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _fallTimeoutDelta = FallTimeout;
            _xAnimatorParam = Animator.StringToHash("xDir");
            _zAnimatorParam = Animator.StringToHash("zDir");
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            MoveAndRotate();
            PlayWalkAudio();

            horizontalVelocity = transform.InverseTransformVector(_controller.velocity);

            animator.SetInteger(_xAnimatorParam, GetAnimationParameterValue(horizontalVelocity.x));
            animator.SetInteger(_zAnimatorParam,GetAnimationParameterValue(horizontalVelocity.z));
        }

        private int GetAnimationParameterValue(float value)
        {
            if (value > 0)
                return value < movementThreshold ? 0 : 1;


            return Mathf.Abs(value) < movementThreshold ? 0 : -1;
        }

        private void PlayWalkAudio()
        {
            if (isWalking && !footstepAudio.isPlaying)
            {
                footstepAudio.Play();
            }
            else if (!isWalking && footstepAudio.isPlaying)
            {
                footstepAudio.Stop();
            }
        }

        private Vector3 GetTargetRotationVector()
        {
            return (_input.MousePositionInWorldSpace - transform.position).normalized;
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            var spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void MoveAndRotate()
        {
            var targetSpeed = _input.Move == Vector2.zero
                ? 0.0f
                : MoveSpeed;
            MovePlayer(targetSpeed);
            RotatePlayer();
        }

        private void MovePlayer(float targetSpeed)
        {
            var speed = CalculateCurrentMovementSpeed(targetSpeed);

            // move the player
            var moveDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
            _controller.Move(moveDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            isWalking = speed > 0.2;
        }

        private float CalculateCurrentMovementSpeed(float targetSpeed)
        {
            var currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            const float speedOffset = 0.1f;
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                var speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                return Mathf.Round(speed * 1000f) / 1000f;
            }

            return targetSpeed;
        }

        private void RotatePlayer()
        {
            var targetRotationVector = GetTargetRotationVector();
            _targetRotation = Mathf.Atan2(targetRotationVector.x, targetRotationVector.z) * Mathf.Rad2Deg;

            // rotate to face mouse cursor
            transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}