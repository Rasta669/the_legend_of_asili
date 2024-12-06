using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main; // use the main camera on player object
    }
    private void LateUpdate()
    {

        transform.LookAt(transform.position + cam.transform.forward);
    }
}
