using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody _thisRB;

    private void Start()
    {
        _thisRB = GetComponent<Rigidbody>();
    }
    public void Knockback(Vector3 direction)
    {
        _thisRB?.AddForce(direction * 10f, ForceMode.Impulse);
    }
}
