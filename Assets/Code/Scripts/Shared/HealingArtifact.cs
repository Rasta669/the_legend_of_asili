using UnityEngine;

public class HealingArtifact : MonoBehaviour
{
    [SerializeField] private int _healAmount;
    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerStats>().Heal(_healAmount);
                HandleAbsorption();
                Destroy(gameObject);
            }
        }
    }

    private void HandleAbsorption()
    {
        // play some animations
        // play sound
        Debug.Log("Absorbed healing artifact!");
    }
}
