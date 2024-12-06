//using UnityEngine;

//public class AnimationController : MonoBehaviour
//{
//    Animator animator;
//    [SerializeField] float blend = 0.5f;
//    [SerializeField] float stopBlend = 0.0f;
     
//    FieldOfView fov;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        fov = gameObject.GetComponentInParent<FieldOfView>(); ;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Debug.Log($"hasArrived: {Enemies.hasArrived}");
//        Debug.Log($"speed: {animator.GetFloat("speed")}");
//        Debug.Log($"Attack: {animator.GetFloat("AttackParameter")}");
//        if (!fov.canSeePlayer)
//        {
//            animator.SetFloat("AttackParameter", 0.0f);
//            if (Enemies.hasArrived == false)
//            {
//                //animator.SetBool("isWalking", true);
//                animator.SetFloat("speed", blend);
//            }
//            else
//            {
//                //animator.SetBool("isWalking", false);
//                animator.SetFloat("speed", stopBlend);
//            }
//        }
//        //else
//        //    animator.SetFloat("Blend", runBlend);
//    }
//}

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
