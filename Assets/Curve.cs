using System.Collections;
using UnityEngine;

public class Curve : MonoBehaviour
{
    public GameObject target;
    public float time;
    public float height;

    private void Start() => StartCoroutine(Run(time));

    private static float GetPosition(float x) => -4 * (x - .5f) * (x - .5f) + 1;

    private IEnumerator Run(float duration)
    {
        var startPos = transform.position;
        var targetPos = target.transform.position;
        var diff = targetPos - startPos;
        
        var additionPos = Vector3.zero;
        for (var i = 0f; i <= duration; i += Time.deltaTime)
        {
            yield return null;
            var ratio = i / duration;
            additionPos.x = diff.x * ratio;
            additionPos.y = GetPosition(ratio) * (height + diff.y) + diff.y * ratio;
            additionPos.z = diff.z * ratio;
            transform.position = startPos + additionPos;
        }

        transform.position = targetPos;
    }
}