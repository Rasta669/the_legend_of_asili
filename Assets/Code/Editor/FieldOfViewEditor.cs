using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        // Draw field of view radius
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        // Draw field of view angles
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        // Draw line to the player if they are visible
        if (fov.canSeePlayer && fov.playerRef != null)
        {
            Handles.color = Color.black;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }

        // Draw attack range
        EnemyFollow enemyFollow = fov.GetComponent<EnemyFollow>();
        if (enemyFollow != null)
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, enemyFollow.hitDistance);

            Handles.Label(
                fov.transform.position + Vector3.left * enemyFollow.hitDistance,
                "Attack Range",
                new GUIStyle { normal = { textColor = Color.red }, fontStyle = FontStyle.Bold }
            );
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

