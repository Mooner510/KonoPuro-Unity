using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class SelectManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI captionText;
    private Camera cam;
    private TimelineManager timelineManager;
    private PlayableDirector director;
    private string caption;
    
    [SerializeField] private Material selectMaterial;
    private Queue<MeshRenderer> meshQueue = new();
    
    private void Awake()
    {
        cam = Camera.main;
        timelineManager = FindObjectOfType<TimelineManager>();
        director = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (timelineManager.stateStack.TryPeek(out var state) && state == Scenestate.Lobby)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit) && hit.transform.gameObject.CompareTag("Selectable"))
            {
                SetMaterial(hit);
                
                var cap = hit.transform.gameObject.name;
                if (caption != cap)
                {
                    SetCaption(cap);
                }
                UpdateCaption(Time.deltaTime);
                if (Input.GetMouseButtonDown(0))
                {
                    timelineManager.PlayTimeline(Scenestate.Lobby, Enum.TryParse(cap, out Scenestate state2) ? state2 : Scenestate.Obj1);
                }
            }
            else
            {
                ReSetMaterial();
                UpdateCaption(-Time.deltaTime);
            }
        }
        else
        {
            caption = null;
            ReSetMaterial();
            UpdateCaption(-Time.deltaTime);
        }
    }

    private void UpdateCaption(float delta)
    {
        director.time = Mathf.Clamp((float)director.time + delta, 0, (float)director.duration);
    }
    
    private void ReSetMaterial()
    {
        while (meshQueue.Count > 0)
        {
            var dequeue = meshQueue.Dequeue();
            dequeue.materials = new[] { dequeue.materials[0] };
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SetMaterial(RaycastHit hit)
    {
        var meshRenderer = hit.transform.GetComponent<MeshRenderer>();
        meshQueue.Enqueue(meshRenderer);
        meshRenderer.materials = new[] { meshRenderer.materials[0], selectMaterial };
    }
    
    private void SetCaption(string cap)
    {
        caption = cap;
        captionText.color = new Color(1, 1, 1, 0);
        captionText.text = $"> {cap} <";
        
        director.time = 0;
        director.Play();
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }
}