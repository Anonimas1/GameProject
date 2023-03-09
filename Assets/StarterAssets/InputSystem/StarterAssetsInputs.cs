using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;

        [Header("Character Input Values")] 
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")] public bool analogMovement;

        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private InputAction _moveAction;
        private InputAction _lookAction;

        public void Awake()
        {
            inputActionAsset.Enable();
            var actionMap = inputActionAsset.FindActionMap("Player");

            _moveAction = actionMap.FindAction("Move");
            _lookAction = actionMap.FindAction("Look");
            actionMap.FindAction("Jump").performed += OnJump;
            actionMap.FindAction("Sprint").performed += OnSprint;
        }

        private void Update()
        {
            if (cursorInputForLook)
            {
                LookInput(_lookAction.ReadValue<Vector2>());
            }
            MoveInput(_moveAction.ReadValue<Vector2>());
        }
        
        private void OnJump(InputAction.CallbackContext ctx)
        {
            JumpInput();
        }
        private void OnSprint(InputAction.CallbackContext ctx)
        {
            SprintInput();
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput()
        {
            jump = true;
        }

        public void SprintInput()
        {
            sprint = !sprint;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private static void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}