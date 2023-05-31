using System.Linq;
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
        public Vector2 MousePositionOnScreen;
        public UnityEvent OnPause;

        [Header("Firing settings")]
        public bool Fire;

        public UnityEvent OnNextWeapon;
        public UnityEvent OnPrevWeapon;
        public UnityEvent OnRepair;
        

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
            actionMap.FindAction("Fire").performed += OnFire;
            actionMap.FindAction("Pause").performed += (_) => OnPause.Invoke();
            actionMap.FindAction("NextWeapon").performed += (_) => OnNextWeapon.Invoke();
            actionMap.FindAction("PrevWeapon").performed += (_) => OnPrevWeapon.Invoke();
            actionMap.FindAction("Repair").performed += (_) => OnRepair.Invoke();
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
            var hits = Physics.RaycastAll(ray, 1000, LayerMask.GetMask("Ground")).FirstOrDefault();
            
            {
                Debug.DrawLine(ray.origin, hits.point, Color.blue);
                MousePositionInWorldSpace = hits.point;
            }
        }

        public void OnFire(InputAction.CallbackContext ctx)
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
    }
}