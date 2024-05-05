using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardMove : MonoBehaviour {
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    public IEnumerator Move(Vector3 start, Transform end) {
        float t=0;
        float newTime = Random.Range(minTime, maxTime);
        
        while (t < newTime) {
            t += Time.deltaTime;
            
            transform.position = Vector3.Lerp(start, end.position, t/newTime);
            
            yield return null;
        }
    }
}
