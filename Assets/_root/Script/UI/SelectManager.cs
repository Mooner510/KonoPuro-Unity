using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace _root.Script.UI
{
    public class SelectManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI captionText;
        private Camera cam;
        private string caption;
        private PlayableDirector director;
        private TimelineManager timelineManager;

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
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit) && hit.transform.gameObject.CompareTag("Selectable"))
                {
                    var cap = hit.transform.gameObject.name;
                    if (caption != cap) SetCaption(cap);
                    UpdateCaption(Time.deltaTime);
                    if (Input.GetMouseButtonDown(0))
                        timelineManager.PlayTimeline(Scenestate.Lobby,
                            Enum.TryParse(cap, out Scenestate state2) ? state2 : Scenestate.Obj1);
                }
                else
                {
                    UpdateCaption(-Time.deltaTime);
                }
            }
            else
            {
                caption = null;
                UpdateCaption(-Time.deltaTime);
            }
        }

        private void UpdateCaption(float delta)
        {
            director.time = Mathf.Clamp((float)director.time + delta, 0, (float)director.duration);
        }

        private void SetCaption(string cap)
        {
            caption = cap;
            director.time = 0;
            captionText.color = new Color(1, 1, 1, 0);
            captionText.text = $"> {cap} <";
            director.Play();
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
    }
}