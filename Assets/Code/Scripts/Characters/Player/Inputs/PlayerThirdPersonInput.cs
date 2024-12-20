using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)] // makes this script run before the ones that use it
public class PlayerThirdPersonInput : MonoBehaviour, PlayerControls.IThirdPersonMapActions
{


    public Vector2 ScrollInput { get; private set; }
    [SerializeField] private CinemachineCamera _virtualCamera;
    [SerializeField] private float _cameraZoomSpeed = 0.1f;
    [SerializeField] private float _camMinZoom = 1f;
    [SerializeField] private float _camMaxZoom = 5f;


    private CinemachineThirdPersonFollow _thirdPersonFollow;

    #region STARTUP

    private void Awake()
    {
        _thirdPersonFollow = _virtualCamera.GetComponent<CinemachineThirdPersonFollow>();
    }
    private void OnEnable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.Log("Player controls is not initialized.");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Enable();
        PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.SetCallbacks(this);
    }
    private void OnDisable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.Log("Player controls is not initialized.");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Disable(); ;
        PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.RemoveCallbacks(this);
    }
    #endregion STARTUP



    #region METHODS

    private void Update()
    {
        _thirdPersonFollow.CameraDistance = Mathf.Clamp(_thirdPersonFollow.CameraDistance + ScrollInput.y, _camMinZoom, _camMaxZoom);
    }

    private void LateUpdate()
    {
        ScrollInput = Vector2.zero;
    }
    public void OnScrollCamera(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        Vector2 scrollInput = context.ReadValue<Vector2>();
        ScrollInput = _cameraZoomSpeed * -1f * scrollInput.normalized;
    }
    #endregion METHODS
}
