using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    [SerializeField] float blend = 0.9f;
    [SerializeField] float stopBlend = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"hasArrived: {Enemies.hasArrived}");
        Debug.Log($"Blend: {animator.GetFloat("Blend")}");
        if (Enemies.hasArrived == false)
        {
            //animator.SetBool("isWalking", true);
            animator.SetFloat("Blend", blend);
        }
        else
        {
            //animator.SetBool("isWalking", false);
            animator.SetFloat("Blend", stopBlend);
        }
    }
}
