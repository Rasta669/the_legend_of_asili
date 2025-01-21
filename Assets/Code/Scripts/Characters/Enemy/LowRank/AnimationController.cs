using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float blend = 0.5f;
    [SerializeField] private float stopBlend = 0.0f;
    [SerializeField] private float walkBlend = 0.5f;
    [SerializeField] private float runBlend = 1.0f;

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
            //idle
            //animator.SetFloat("Speed", 0.0f);
            BeIdle();   
            // walking
            if (!enemy.hasArrived)
            {
                //animator.SetFloat("Speed", blend);
                Walk();
            }
            else
            {
                //animator.SetFloat("Speed", stopBlend);
                BeIdle();
            }
        }
        // else logic for when fov.canSeePlayer is true can be added here
    }

    void BeIdle()
    {
        animator.SetFloat("Speed", 0.0f);
    }

    void Walk()
    {
        animator.SetFloat("Speed", walkBlend);
    }

    void Run()
    {
        animator.SetFloat("Speed", runBlend);
    }
}
