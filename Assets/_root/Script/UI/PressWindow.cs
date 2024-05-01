using UnityEngine;

public class PressWindow : MonoBehaviour
{ 
    private string input = "";
    private new Animation animation;
    private TimelineManager timelineManager;
    private bool OnPress = false;
    private void Awake()
    {
        timelineManager = FindObjectOfType<TimelineManager>();
    }

    private void Update()
    {
        input = Input.inputString;
        if (input == "")
        {
            input = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) ? "mouse" : "";
        }

        if (input == "" || OnPress)
        {
            return;
        }
        
        timelineManager.PlayTimeline(timelineManager.stateStack.Peek(), Scenestate.Lobby);
    }
}