using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour, PlayerControls.IPlayerActionsMapActions
{
    #region FIELDS
    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    public bool AttackInput { get; private set; }
    public bool hasAttacked = false;
    public bool InteractInput { get; private set; }
    #endregion FIELDS


    private MeleeWeapon _meleeWeapon;
    #region STARTUP

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerState = GetComponent<PlayerState>();
        _meleeWeapon = GetComponentInChildren<MeleeWeapon>();
    }
    private void OnEnable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.Log("Player controls is not initialized.");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.PlayerActionsMap.Enable();
        PlayerInputManager.Instance.PlayerControls.PlayerActionsMap.SetCallbacks(this);
    }
    private void OnDisable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.Log("Player controls is not initialized.");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.PlayerActionsMap.Disable(); ;
        PlayerInputManager.Instance.PlayerControls.PlayerActionsMap.RemoveCallbacks(this);
    }
    #endregion STARTUP


    #region ACTIONS

    private void Update()
    {
        if (_playerLocomotionInput.MoveInput != Vector2.zero || _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping || _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling)
        {
            InteractInput = false;
        }
    }

    public void SetInteractFalse()
    {
        Debug.Log("Set Interact to false");
        InteractInput = false;
    }
    public void SetAttackingFalse()
    {
        Debug.Log("Set Attack to false");
        AttackInput = false; // Reset attack input
        _meleeWeapon.isReadyToAttack = true; // Allow the next attack
        _meleeWeapon.CycleAttack(); // Cycle to the next attack animation
    }


    public void SetHasAttacked()
    {
        Debug.Log("Set HasAttacked to true");
        hasAttacked = true; // Only set to true
    }

    public void SetHasAttackedFalse()
    {
        Debug.Log("Set HasAttacked to false");
        hasAttacked = false; // Reset to false after attack finishes
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        AttackInput = true;
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        InteractInput = true;
    }
    #endregion ACTIONS
}
