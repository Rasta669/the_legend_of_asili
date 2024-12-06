using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)] // makes this script run before the ones that use it
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerActions
{

    #region FIELDS
    [SerializeField] private bool holdToSprint = true;
    public bool SprintToggledOn { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    #endregion FIELDS


    #region STARTUP
    private void OnEnable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.Log("Player controls is not initialized.");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.Player.Enable();
        PlayerInputManager.Instance.PlayerControls.Player.SetCallbacks(this);
    }
    private void OnDisable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.Log("Player controls is not initialized.");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.Player.Disable(); ;
        PlayerInputManager.Instance.PlayerControls.Player.RemoveCallbacks(this);
    }


    private void LateUpdate()
    {
        JumpPressed = false;
    }


    #endregion STARTUP
    public void OnCrouch(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    #region METHODS
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        JumpPressed = true;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintToggledOn = holdToSprint || !SprintToggledOn;
        }
        else if (context.canceled)
        {
            SprintToggledOn = !holdToSprint && SprintToggledOn;
        }
    }
    #endregion METHODS

}
