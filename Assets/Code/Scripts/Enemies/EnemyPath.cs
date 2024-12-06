using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EnemyPath : MonoBehaviour
{
    public static Transform[] path;
    public GameObject Path;
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
}
