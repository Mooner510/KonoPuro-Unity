using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class FirstCard : MonoBehaviour {
    private Camera camera; 
    private GameObject todayscard;
    private void Awake()
    {
        todayscard = GameObject.Find("Todayscard");
    }

    private void Start()
    {
        todayscard.SetActive(false);
        camera = Camera.main;
    }

    void Update(){
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Physics.Raycast(ray, out var hit, 20, 1 << 6))
            {
                todayscard.SetActive(true);
            }
        }
    }
}
