using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float blend = 0.5f;
    [SerializeField] private float stopBlend = 0.0f;

    private FieldOfView fov;
    [SerializeField] private Enemies enemy; // Reference to the associated Enemies instance

    void Start()
    {
        animator = GetComponent<Animator>();
        fov = gameObject.GetComponentInParent<FieldOfView>();

        // If not assigned in the inspector, try to find the Enemies component in parent
        if (enemy == null)
        {
            enemy = GetComponent<Enemies>();
            if (enemy == null)
            {
                Debug.LogError("No Enemies component found for AnimationController!");
            }
        }
    }

    void Update()
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy reference is missing.");
            return;
        }

        if (!fov.canSeePlayer)
        {
            animator.SetFloat("AttackParameter", 0.0f);
            if (!enemy.hasArrived)
            {
                animator.SetFloat("speed", blend);
            }
            else
            {
                animator.SetFloat("speed", stopBlend);
            }
        }
        // else logic for when fov.canSeePlayer is true can be added here
    }
}
