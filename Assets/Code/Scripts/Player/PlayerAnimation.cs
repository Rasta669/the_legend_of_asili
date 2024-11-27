using System;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _locomotionBlendSpeed = 0.02f;
    private PlayerLocomotionInput _playerLocomotionInput;
    private PlayerState _playerState;
    private PlayerController _playerController;
    private PlayerInputActions _playerInputActions;

    // locomotion params
    private static int inputXHash = Animator.StringToHash("InputX");
    private static int inputYHash = Animator.StringToHash("InputY");
    private static int inputMagnitude = Animator.StringToHash("InputMagnitude");
    private static int isGroundedHash = Animator.StringToHash("isGrounded");
    private static int isFallingHash = Animator.StringToHash("isFalling");
    private static int isJumpingHash = Animator.StringToHash("isJumping");
    private static int isIdlingHash = Animator.StringToHash("isIdling");

    // camera params
    private static int isRotatingToTargetHash = Animator.StringToHash("isRotatingToTarget");
    private static int rotationMismatchHash = Animator.StringToHash("rotationMismatch");

    // action  params
    private static int isAttackingHash = Animator.StringToHash("isAttacking");
    private static int attackCountHashed = Animator.StringToHash("attackCount");

    private static int isInteractingHash = Animator.StringToHash("isInteracting");
    private static int isPlayingActionHash = Animator.StringToHash("isPlayingAction");
    private int[] actionHashes;

    // 
    private Vector3 _currentBlendInput = Vector3.zero;
    private MeleeWeapon _weapon;
    // methods
    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        _playerState = GetComponent<PlayerState>();
        _playerController = GetComponent<PlayerController>();
        _playerInputActions = GetComponent<PlayerInputActions>();
        _weapon = GetComponentInChildren<MeleeWeapon>();

        actionHashes = new int[] { isInteractingHash };
    }

    private void Update()
    {
        UpdateAnimState();
    }

    private void UpdateAnimState()
    {

        bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
        bool isWalking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Walking;
        bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
        bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
        bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
        bool isGrounded = _playerState.IsGroundedState();
        bool isPlayingAction = actionHashes.Any(hash => _animator.GetBool(hash));




        //
        bool isJumpFallBlendValue = isJumping || isFalling;
        // Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MoveInput * 1.5f : isJumpFallBlendValue ? _playerLocomotionInput.MoveInput * 1f : _playerLocomotionInput.MoveInput;
        Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MoveInput * 1.5f : _playerLocomotionInput.MoveInput;
        _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, _locomotionBlendSpeed * Time.deltaTime);


        _animator.SetBool(isGroundedHash, isGrounded);
        _animator.SetBool(isIdlingHash, isIdling);
        _animator.SetBool(isJumpingHash, isJumping);
        _animator.SetBool(isFallingHash, isFalling);
        _animator.SetBool(isRotatingToTargetHash, _playerController.IsRotatingToTarget);

        _animator.SetBool(isAttackingHash, _playerInputActions.AttackInput);
        _animator.SetInteger(attackCountHashed, _weapon.AttackCount);

        _animator.SetBool(isInteractingHash, _playerInputActions.InteractInput);
        _animator.SetBool(isPlayingActionHash, isPlayingAction);

        _animator.SetFloat(inputXHash, _currentBlendInput.x);
        _animator.SetFloat(inputYHash, _currentBlendInput.y);
        _animator.SetFloat(inputMagnitude, _currentBlendInput.magnitude);
        _animator.SetFloat(rotationMismatchHash, _playerController.RotationMismatch);
    }

}
