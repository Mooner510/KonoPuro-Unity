using System.Collections;
using UnityEngine;

public class MoveCommision : MonoBehaviour
{
    [SerializeField] private Transform      targetTransform;
    [SerializeField] private float          time;
    [SerializeField] private float          yPosition;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Move());
    }

    private float GetYRatio(float timer) =>
        timer < time * .5f ? timer * 2 / time : 1 - (timer - time * .5F) * 2 / time;
    private IEnumerator Move()
    {
        Vector3 start = transform.position;
        Vector3 end   = targetTransform.position;
        float   timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float yAlpha = GetYRatio(timer);
            float x      = Mathf.Lerp(start.x, end.x, timer / time);
            float y      = Mathf.Lerp(start.y, start.y + yPosition, Mathf.Lerp(yAlpha, 1, yAlpha));
            transform.position = new Vector3(x, y, 0);
            yield return null;
        }
        transform.position = targetTransform.position;
    }
}