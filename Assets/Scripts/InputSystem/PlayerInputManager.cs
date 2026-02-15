using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheFrozenBanana
{
    public class PlayerInputManager : MonoBehaviour, IInputManager3d, Controls.IPlayerActions
    {
        [SerializeField] protected bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        public event Action JumpEvent;
        public event Action DodgeEvent;
        [field: SerializeField] public Vector2 MovementValue { get; private set; }
        private Controls controls;

        // Movement
        public bool IsMovementEnabled = true;
        private float _horizontal;
        private float _vertical;
        private float _pitch;
        private float _yaw;
        private bool _isToggleInventory;
        private bool _isJump;
        private bool _isJumpCancelled;
        private bool _isDash;
        private bool _isAttack;
        [SerializeField] private bool _isEnabled;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Start()
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }

        private void OnDestroy()
        {
            controls.Player.Disable();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsEnabled)
            {
                //_horizontal = Input.GetAxisRaw("Horizontal");
                //_vertical = Input.GetAxisRaw("Vertical");

                //_pitch = Input.GetAxisRaw("Mouse Y");
                //_yaw = Input.GetAxisRaw("Mouse X");

                _isJump = Input.GetButtonDown("Jump");
                _isJumpCancelled = Input.GetButtonUp("Jump");

                _isDash = Input.GetButton("Fire3");

                _isAttack = Input.GetButtonDown("Fire1");

                _isToggleInventory = Input.GetKeyDown(KeyCode.I);

                if (_showDebugLog)
                {
                    Debug.Log("Horizontal: " + _horizontal);
                    Debug.Log("Vertical: " + _vertical);
                    Debug.Log("IsJumping: " + _isJump);
                    Debug.Log("IsJumpCancelled: " + _isJumpCancelled);
                    Debug.Log("IsDashing: " + _isDash);
                    Debug.Log("IsAttacking: " + _isAttack);
                    Debug.Log("IsToggleInventory: " + _isToggleInventory);
                }
            }
            else
            {
                //_horizontal = 0;
                //_vertical = 0;
                //_pitch = 0;
                //_yaw = 0;
                _isToggleInventory = false;
                _isJump = false;
                _isJumpCancelled = false;
                _isDash = false;
                _isAttack = false;
            }
        }

        #region New Input System
        public void OnMove(InputAction.CallbackContext context)
        {
            _horizontal = context.ReadValue<Vector2>().x;
            _vertical = context.ReadValue<Vector2>().y;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _pitch = context.ReadValue<Vector2>().y;
            _yaw = context.ReadValue<Vector2>().x;
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            JumpEvent?.Invoke();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            DodgeEvent?.Invoke();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            
        }
        #endregion

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public float Horizontal { get { return _horizontal; } }

        public float Vertical { get { return _vertical; } }

        public float Pitch { get { return _pitch; } }

        public float Yaw { get { return _yaw; } }

        public bool IsToggleInventory { get { return _isToggleInventory; } }

        public bool IsJump { get { return _isJump; } }

        public bool IsJumpCancelled { get { return _isJumpCancelled; } }

        public bool IsDash { get { return _isDash; } }

        public bool IsAttack { get { return _isAttack; } }

        public bool IsEnabled 
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
            }
        }

        public bool IsStrafe => throw new System.NotImplementedException();

        public bool IsSwitchWeapon => throw new System.NotImplementedException();

        public bool IsRunning => throw new System.NotImplementedException();

        public Vector3 CurrentTarget => throw new System.NotImplementedException();
    }
}
