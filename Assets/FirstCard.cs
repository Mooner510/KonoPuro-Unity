using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class ExampleScript : MonoBehaviour {
    public Camera camera;
    [SerializeField] private GameObject todayscard;
    

    private void Start()
    {
        Debug.Log(gameObject.name);
        todayscard.SetActive(false);
    }

    void Update(){
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 15f, 1 << 6)) {
                todayscard.SetActive(true);
            }
        }
    }
}
