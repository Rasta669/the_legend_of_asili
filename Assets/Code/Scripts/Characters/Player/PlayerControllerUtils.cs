using UnityEngine;

public static class PlayerControllerUtils
{
    public static Vector3 GetNormalWithSphereCast(CharacterController characterController, LayerMask layerMask = default)
    {
        Vector3 normal = Vector3.up;
        Vector3 center = characterController.transform.position + characterController.center;
        float distance = characterController.height / 2f + characterController.stepOffset + 0.01f;



        if (Physics.SphereCast(center, characterController.radius, Vector3.down, out RaycastHit hit, distance, layerMask))
        {
            normal = hit.normal;
        }
        return normal;
    }
}

