using System;
using UnityEngine;
using UnityEngine.Events;
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
        public bool Reload;
        public bool PlaceBarrel;
        public UnityEvent OnPause;
        
        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Firing settings")]
        public bool Fire;

        public UnityEvent OnNextWeapon;
        public UnityEvent OnPrevWeapon;
        

        [Header("Calculated values")]
        public Vector3 MousePositionInWorldSpace;

        private InputAction _moveAction;
        private InputAction _mouseInputAction;
        
        public void Start()
        {
            inputActionAsset.Enable();
            var actionMap = inputActionAsset.FindActionMap("Player");

            _moveAction = actionMap.FindAction("Move");
            _mouseInputAction = actionMap.FindAction("Look");
            actionMap.FindAction("Jump").performed += OnJump;
            actionMap.FindAction("Sprint").performed += OnSprint;
            actionMap.FindAction("Fire").performed += OnFire;
            actionMap.FindAction("Reload").performed += OnReload;
            actionMap.FindAction("Pause").performed += (_) => OnPause.Invoke();
            actionMap.FindAction("PlaceBarrel").performed += OnBarrelPlace;
            actionMap.FindAction("NextWeapon").performed += (_) => OnNextWeapon.Invoke();
            actionMap.FindAction("PrevWeapon").performed += (_) => OnPrevWeapon.Invoke();
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
                Debug.DrawLine(ray.origin, hintData.point, Color.blue);
                MousePositionInWorldSpace = hintData.point;
            }
        }

        public void OnBarrelPlace(InputAction.CallbackContext obj)
        {
            PlaceBarrelInput();
        }

        public void OnReload(InputAction.CallbackContext obj)
        {
            ReloadInput(true);
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            JumpInput();
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            SprintInput();
        }

        public void OnFire(InputAction.CallbackContext ctx)
        {
            FireInput(ctx.ReadValueAsButton());
        }

        public void ReloadInput(bool input)
        {
            Reload = input;
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
        
        public void PlaceBarrelInput()
        {
            PlaceBarrel = true;
        }
    }
}