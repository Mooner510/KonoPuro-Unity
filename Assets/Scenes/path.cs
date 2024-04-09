using System.Collections;
using UnityEngine;

public class path : MonoBehaviour
{
    public Transform target;

    public Transform wall;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3.Distance(Vector3.zero, Vector3.zero);
        yield break;
    }
}