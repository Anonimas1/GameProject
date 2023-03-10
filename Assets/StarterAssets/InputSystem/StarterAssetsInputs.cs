using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        
        [Header("Character Input Values")]
        public Vector2 Move;
        [FormerlySerializedAs("MousePosition")]
        public Vector2 MousePositionOnScreen;
        public bool Jump;
        public bool Sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Firing settings")]
        public bool Fire;

        [Header("Calculated values")]
        public Vector3 MousePositionInWorldSpace;

        private InputAction _moveAction;
        private InputAction _mouseInputAction;

        public void Awake()
        {
            inputActionAsset.Enable();
            var actionMap = inputActionAsset.FindActionMap("Player");

            _moveAction = actionMap.FindAction("Move");
            _mouseInputAction = actionMap.FindAction("Look");
            actionMap.FindAction("Jump").performed += OnJump;
            actionMap.FindAction("Sprint").performed += OnSprint;
            actionMap.FindAction("Fire").performed += OnFire;
        }

        private void Update()
        {
            LookInput(_mouseInputAction.ReadValue<Vector2>());
            MoveInput(_moveAction.ReadValue<Vector2>());
            
            CalculateMousePositionInWorldSpace();
        }

        private void CalculateMousePositionInWorldSpace()
        {
            var ray = Camera.main.ScreenPointToRay(MousePositionOnScreen);
            if (Physics.Raycast(ray, out var hintData, 1000))
            {
                MousePositionInWorldSpace = hintData.point;
            }
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            JumpInput();
        }

        private void OnSprint(InputAction.CallbackContext ctx)
        {
            SprintInput();
        }

        private void OnFire(InputAction.CallbackContext ctx)
        {
            FireInput(ctx.ReadValueAsButton());
        }

        public void FireInput(bool input)
        {
            Fire = input;
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            MousePositionOnScreen = newLookDirection;
        }

        public void JumpInput()
        {
            Jump = true;
        }

        public void SprintInput()
        {
            Sprint = !Sprint;
        }
    }
}