using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _playerCamera;
    public float RotationMismatch { get; private set; } = 0f;
    public bool IsRotatingToTarget { get; private set; } = false;


    [Header("ENVIRONMENTAL DETAILS")]
    [SerializeField] private LayerMask _groundLayers;


    [Header("MOVEMENT SETTINGS")]
    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float sprintAcceleration = 0.5f;
    public float sprintSpeed = 7f;
    public float inAirAcceleration = 0.15f;
    public float drag = 0.1f;
    public float inAirDrag = 5f;
    public float gravity = 25f;
    public float terminalVelocity = 50f;
    public float jumpSpeed = 1.0f;
    public float movingThreshold = 0.01f;

    [Header("ANIMATIONS SETTINGS")]
    public float playerModelRotationSpeed = 10f;
    public float rotateToTargetTime = 0.25f;

    [Header("LOOK SETTINGS")]
    public float lookSenseH = 0.1f;
    public float lookSenseV = 0.1f;
    public float lookLimitV = 79f;
    public float rotationTolerance = 90f;
    private Vector2 _camRotation = Vector2.zero;
    private Vector2 _targetRotation = Vector2.zero;


    private bool _jumpedLastFrame = false;
    private bool _isRotatingClockwise = false;
    private float _rotatingToTargetTimer = 0f;
    private float _verticalVelocity = 0f;
    private float _antiBump;
    private float _stepOffset;

    private PlayerMovementState _lastMovementState = PlayerMovementState.Falling;



    #region  STARTUP
    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _antiBump = sprintSpeed;
        _stepOffset = _characterController.stepOffset;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion STARTUP


    #region METHODS
    private void Update()
    {
        UpdateMovementState();
        HandleVerticalMovement();
        HandleLateralMovement();
    }


    private void LateUpdate()
    {
        UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        _camRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
        _camRotation.y = Mathf.Clamp(_camRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

        // rotate player
        _targetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;

        bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
        IsRotatingToTarget = _rotatingToTargetTimer > 0;

        // ROTATE if we're not idling
        if (!isIdling)
        {
            RotatePlayerToTarget();
        }
        // If rotation mismatch not within tolerance, or rotate to target is active, ROTATE
        else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotatingToTarget)
        {
            UpdateIdleRotation(rotationTolerance);
        }

        _playerCamera.transform.rotation = Quaternion.Euler(_camRotation.y, _camRotation.x, 0f);

        // Get angle between camera and player
        Vector3 camForwardProjectedXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 crossProduct = Vector3.Cross(transform.forward, camForwardProjectedXZ);
        float sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
        RotationMismatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);
    }
    private void UpdateIdleRotation(float rotationTolerance)
    {
        // Initiate new rotation direction
        if (Mathf.Abs(RotationMismatch) > rotationTolerance)
        {
            _rotatingToTargetTimer = rotateToTargetTime;
            _isRotatingClockwise = RotationMismatch > rotationTolerance;
        }
        _rotatingToTargetTimer -= Time.deltaTime;

        // Rotate player
        if (_isRotatingClockwise && RotationMismatch > 0f ||
            !_isRotatingClockwise && RotationMismatch < 0f)
        {
            RotatePlayerToTarget();
        }
    }

    private void RotatePlayerToTarget()
    {
        Quaternion targetRotationX = Quaternion.Euler(0f, _targetRotation.x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, playerModelRotationSpeed * Time.deltaTime);
    }
    private void HandleLateralMovement()
    {
        // local reference for sprint handler
        bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isGrounded = _playerState.IsGroundedState();

        // state dependent acceleration and speed
        float lateralAcceleration = !isGrounded ? inAirAcceleration : isSprinting ? sprintAcceleration : runAcceleration;
        float clampLateralMagnitude = !isGrounded ? sprintSpeed : isSprinting ? sprintSpeed : runSpeed;

        //
        Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 moveDirection = cameraRightXZ * _playerLocomotionInput.MoveInput.x + cameraForwardXZ * _playerLocomotionInput.MoveInput.y;

        // move per frame
        Vector3 moveDelta = moveDirection * lateralAcceleration; //* Time.deltaTime;
        Vector3 newVelocity = _characterController.velocity + moveDelta;

        // add drag to avoid sliding
        float dragMagnitude = isGrounded ? drag : inAirDrag;
        Vector3 currentDrag = newVelocity.normalized * dragMagnitude; // * Time.deltaTime;
        newVelocity = (newVelocity.magnitude > dragMagnitude) ? newVelocity - currentDrag : Vector3.zero; //  * Time.deltaTime 
        newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), clampLateralMagnitude);
        newVelocity.y += _verticalVelocity;
        newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;

        // move character (once per frame)
        _characterController.Move(newVelocity * Time.deltaTime);
    }


    private void HandleVerticalMovement()
    {
        bool isGrounded = _playerState.IsGroundedState();

        _verticalVelocity -= gravity * Time.deltaTime;

        if (isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -_antiBump;

        if (_playerLocomotionInput.JumpPressed && isGrounded)
        {
            _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3f * gravity);
            _jumpedLastFrame = true;
        }

        if (_playerState.IsStateGroundedState(_lastMovementState) && !isGrounded)
        {
            _verticalVelocity += _antiBump;
        }

        if (Mathf.Abs(_verticalVelocity) > Mathf.Abs(terminalVelocity))
            _verticalVelocity = -1f * terminalVelocity;
    }

    private Vector3 HandleSteepWalls(Vector3 velocity)
    {
        Vector3 normal = PlayerControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
        float angle = Vector3.Angle(normal, Vector3.up);
        bool validAngle = angle <= _characterController.slopeLimit;

        if (!validAngle && _verticalVelocity < 0f)
            velocity = Vector3.ProjectOnPlane(velocity, normal);

        return velocity;
    }

    private void UpdateMovementState()
    {
        _lastMovementState = _playerState.CurrentPlayerMovementState;
        bool isMovementInput = _playerLocomotionInput.MoveInput != Vector2.zero;        // order
        bool isMovingLateral = IsMovingLateral();                                       // of calls
        bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLateral;   // matters here
        bool isGrounded = IsGrounded();

        PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting :
                                            isMovingLateral || isMovementInput ?
                                            PlayerMovementState.Walking : PlayerMovementState.Idling;
        _playerState.SetPlayerMovementState(lateralState);


        // control airborn state
        if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y >= 0f)
        {
            _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
            _jumpedLastFrame = false;
            _characterController.stepOffset = 0f;
        }
        else if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y < 0f)
        {
            _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
            _jumpedLastFrame = false;
            _characterController.stepOffset = 0f;
        }
        else
        {
            _characterController.stepOffset = _stepOffset;
        }

    }

    private bool IsGrounded()
    {
        bool grounded = _playerState.IsGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborn();
        return grounded; ;
    }

    private bool IsGroundedWhileGrounded()
    {
        Vector3 sphereCheckPosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);
        bool grounded = Physics.CheckSphere(sphereCheckPosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    private bool IsGroundedWhileAirborn()
    {
        Vector3 normal = PlayerControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
        float angle = Vector3.Angle(normal, Vector3.up);
        bool validAngle = angle <= _characterController.slopeLimit;

        return _characterController.isGrounded && validAngle;

    }

    private bool IsMovingLateral()
    {
        Vector3 lateralVelocity = new(_characterController.velocity.x, 0f, _characterController.velocity.z);
        return lateralVelocity.magnitude > movingThreshold;
    }
    #endregion METHODS
}
