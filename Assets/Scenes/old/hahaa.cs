using System.Collections;
using UnityEngine;

public class hahaa : MonoBehaviour
{
    public Transform target;
    public float time = 3;

    public float h = 5;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float timer = 0;
        var start = transform.position;
        var end = target.position;
        var v_s = start;
        v_s.y += h;
        var v_e = end;
        v_e.y += h;
        var v_m = Vector3.Lerp(v_s, v_e, (float)0.5);
        while (timer < time / 2)
        {
            timer += Time.deltaTime;
            var targetPos1 = Vector3.Lerp(start, v_s, timer / time * 2);
            var targetPos2 = Vector3.Lerp(v_s, v_m, timer / time * 2);
            var targetPos3 = Vector3.Lerp(targetPos1, targetPos2, timer / time * 2);
            transform.position = targetPos3;

            yield return null;
        }

        timer = 0;
        while (timer < time / 2)
        {
            timer += Time.deltaTime;
            var targetPos1 = Vector3.Lerp(v_m, v_e, timer / time * 2);
            var targetPos2 = Vector3.Lerp(v_e, end, timer / time * 2);
            var targetPos3 = Vector3.Lerp(targetPos1, targetPos2, timer / time * 2);
            transform.position = targetPos3;

            yield return null;
        }
    }
}