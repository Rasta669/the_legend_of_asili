using UnityEngine;

public class MinimapCameraLimit : MonoBehaviour
{
    [Header("Follow Object"), Tooltip("Assign the object to follow e.g Player")]
    [SerializeField] private GameObject _followObj;
    [SerializeField] private float _iconHeightSet = 1f;

    private void Start()
    {

        if (_followObj == null)
        {
            Debug.LogError("Player object not found. Make sure the player has the correct tag.");
        }
    }

    private void LateUpdate()
    {
        if (_followObj != null)
        {
            // Follow the player's position
            transform.position = new Vector3(_followObj.transform.position.x, _iconHeightSet, _followObj.transform.position.z);

            // Lock the minimap camera's rotation
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Set fixed rotation (90 degrees top-down view)
        }
    }
}
