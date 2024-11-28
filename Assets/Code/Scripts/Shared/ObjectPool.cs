using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance; // singleton
    [SerializeField] private GameObject _objectPrefab; // set gemPrefabs
    private Queue<GameObject> _objectPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GemFromPool()
    {
        if (_objectPool.Count > 0)
        {
            GameObject obj = _objectPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(_objectPrefab); // create a new gem if pool is empty
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _objectPool.Enqueue(obj);
    }
}
