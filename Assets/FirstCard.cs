using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class FirstCard : MonoBehaviour {
    public Camera camera;
    [SerializeField] private GameObject todayscard;
    [SerializeField] private GameObject authpanel;
  
    private void Start()
    {
        todayscard.SetActive(false);
    }

    void Update(){
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6)&&!authpanel.activeSelf)
            {
                todayscard.SetActive(true);
            }
        }
    }
}
