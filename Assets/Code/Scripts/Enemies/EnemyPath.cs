using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EnemyPath : MonoBehaviour
{
    public static Transform[] path;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void Awake()
    {
        path = new Transform[transform.childCount];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
