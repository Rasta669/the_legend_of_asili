using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    [SerializeField] float blend = 0.5f;
    [SerializeField] float stopBlend = 0.0f;
     
    FieldOfView fov;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        fov = gameObject.GetComponentInParent<FieldOfView>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"hasArrived: {Enemies.hasArrived}");
        Debug.Log($"speed: {animator.GetFloat("speed")}");
        Debug.Log($"Attack: {animator.GetFloat("AttackParameter")}");
        if (!fov.canSeePlayer)
        {
            animator.SetFloat("AttackParameter", 0.0f);
            if (Enemies.hasArrived == false)
            {
                //animator.SetBool("isWalking", true);
                animator.SetFloat("speed", blend);
            }
            else
            {
                //animator.SetBool("isWalking", false);
                animator.SetFloat("speed", stopBlend);
            }
        }
        //else
        //    animator.SetFloat("Blend", runBlend);
    }
}
