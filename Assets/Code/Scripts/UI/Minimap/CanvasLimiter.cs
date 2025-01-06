using UnityEngine;

public class CanvasLimiter : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationLimit = new(0f, 0f, 0f);
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(_rotationLimit); // Set fixed rotation (90 degrees top-down view)
    }
}
