using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public float interactionRange = 3f; // Range to interact
    public float approachRange = 6f; // Range to trigger approach behaviors
    public LayerMask interactableLayer;
    private IInteractable lastApproachedObject; // Keeps track of the last approached object

    private PlayerInputActions _playerInputActions;
    private bool hasPickedUpItem = false;

    private void Awake()
    {
        _playerInputActions = GetComponent<PlayerInputActions>();

    }
    public void Update()
    {
        HandleApproach();

        if (_playerInputActions.InteractInput)
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * 2f; // Offset the raycast origin by 1 unit upwards
        if (Physics.Raycast(raycastOrigin, transform.forward, out RaycastHit hit, interactionRange, interactableLayer))
        {
            // chcek if the hit object is interactable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            // interactable?.Interact(); // if available, interact
            if (interactable != null && hasPickedUpItem)
            {
                interactable.Interact();
            }
        }
    }

    private void HandleApproach()
    {
        // Perform a spherecast to detect nearby interactables
        Collider[] hits = Physics.OverlapSphere(transform.position, approachRange, interactableLayer);
        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        // Handle approach/leave logic
        if (closestInteractable != lastApproachedObject)
        {
            if (lastApproachedObject != null)
                lastApproachedObject.OnPlayerLeave();

            if (closestInteractable != null)
                closestInteractable.OnPlayerApproach();

            lastApproachedObject = closestInteractable;
        }
    }

    // pickup only when midway through the interact animation
    public void SetHasPickedUpItemTrue()
    {
        hasPickedUpItem = true;
    }
    public void SetHasPickedUpItemFalse()
    {
        hasPickedUpItem = false;
    }

    // Draw Gizmos to visualize ranges in the Scene view
    private void OnDrawGizmosSelected()
    {
        // Interaction range (small circle)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        // Approach range (larger circle)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, approachRange);

        // Direction player is facing
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * interactionRange);
    }

}
