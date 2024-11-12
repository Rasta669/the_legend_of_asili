using UnityEngine;

public class ThirdPersonAnimController : MonoBehaviour
{
    // Component references
    private Animator m_Animator;  // Animator component for controlling animations
    private readonly int _hashedSpeedParam = Animator.StringToHash("Speed");  // Hashed parameter name for Speed in Animator
    private Rigidbody m_Rigidbody;  // Rigidbody component for retrieving player velocity

    // Attributes
    [SerializeField] private float maxSpeed = 5f;  // Maximum speed used to normalize animation speed

    // Initialization of components
    private void Start()
    {
        m_Animator = GetComponent<Animator>();    // Reference to Animator component
        m_Rigidbody = GetComponent<Rigidbody>();  // Reference to Rigidbody component
    }

    // Update animation parameters every frame
    private void Update()
    {
        // Set Speed parameter in Animator based on normalized player speed
        m_Animator.SetFloat(_hashedSpeedParam, m_Rigidbody.linearVelocity.magnitude / maxSpeed);
    }
}
