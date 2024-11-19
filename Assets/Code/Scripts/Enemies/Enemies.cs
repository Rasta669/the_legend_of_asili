using UnityEngine;

public class Enemies : MonoBehaviour
{

    [SerializeField] float speed = 5f;
    private int pathPointIndex;
    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = EnemyPath.path[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (pathPointIndex < EnemyPath.path.Length)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(target.position, transform.position) < 0.2f)
            {
                GetNextPoint();
            }
        }


        
    }

    void GetNextPoint()
    {
        if (pathPointIndex < EnemyPath.path.Length-1) {
            pathPointIndex++;
            target = EnemyPath.path[pathPointIndex];

        }
        else
        {
            return;
        }
        
    }
}
