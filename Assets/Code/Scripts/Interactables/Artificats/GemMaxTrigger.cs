using UnityEngine;

public class GemMaxTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _gemsToCollect;
    private int _gemCount = 0;

    private void Start()
    {
        _gemCount = _gemsToCollect.transform.childCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Collect {_gemCount} gems for this level.");
            GemsManager.Instance.SetMaxGems(_gemCount);
            Destroy(gameObject, 0.5f);
        }
    }
}
