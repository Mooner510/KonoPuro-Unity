using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GachaCard : MonoBehaviour {
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    [SerializeField] private float openTime;
    [SerializeField] private GameObject effect;

    private bool opened;
    public bool Opened => opened;
    private bool interectable;

    public void Click() {
        StartCoroutine(Open());
    }

    IEnumerator Open() {
        if (opened || !interectable) yield break;

        effect.SetActive(false);
        opened = true;
        float t=0;
        
        while (t < openTime) {
            t += Time.deltaTime;
            Vector3 newVec = Vector3.Lerp(Vector3.zero, new Vector3(0, 180, 0), t/openTime);
            transform.rotation = Quaternion.Euler(newVec);
            
            yield return null;
        }
    }

    public IEnumerator Move(Vector3 start, Transform end) {
        float t=0;
        float newTime = Random.Range(minTime, maxTime);
        
        while (t < newTime) {
            t += Time.deltaTime;
            
            transform.position = Vector3.Slerp(start, end.position, t/newTime);
            
            yield return null;
        }

        interectable = true;
    }
}
