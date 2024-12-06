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
            HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
            if (hintTextManager != null)
            {
                hintTextManager.ShowHint("Collect the Artifacts");
            }
            Debug.Log($"Collect {_gemCount} gems for this level.");
            GemsManager.Instance.SetMaxGems(_gemCount);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Invoke(nameof(HideHint), 1f);
        }
    }

    private void HideHint()
    {
        Destroy(gameObject);
        HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
        if (hintTextManager != null)
        {
            hintTextManager.HideHint();
        }
    }
}
