using UnityEngine;

public class GemMaxTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _gemsToCollect;
    private int _gemCount = 0;

    private void Start()
    {
        gameObject.SetActive(true);
        if (_gemsToCollect != null)
        {
            _gemCount = _gemsToCollect.transform.childCount;
            GemsManager.Instance.SetMaxGems(_gemCount); // Initialize GemsToCollect here
        }
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
        gameObject.SetActive(false); // Deactivate instead of destroy
        HintTextManager hintTextManager = FindFirstObjectByType<HintTextManager>();
        if (hintTextManager != null)
        {
            hintTextManager.HideHint();
        }
    }
}
