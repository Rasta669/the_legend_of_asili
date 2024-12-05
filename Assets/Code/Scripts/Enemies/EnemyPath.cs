using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EnemyPath : MonoBehaviour
{
    public static Transform[] path;
    public GameObject Path;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void Awake()
    {
        path = new Transform[Path.transform.childCount];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = Path.transform.GetChild(i);
        }

        //path  = new Transform[path.Length];
        //for (int i = 0; i < path.Length; i++) {
        //    path[i] = transform;
        //}

    }

    // Update is called once per frame
    void Update()
    {

    }
}
