using UnityEngine;

public class PressBtn : MonoBehaviour
{ 
    private string input = "";
    private new Animation animation;
    private TimelineManager timelineManager;
    private void Awake()
    {
        timelineManager = FindObjectOfType<TimelineManager>();
    }

    private void OnGUI()
    {
        input = Input.inputString;
        if (input == "")
        {
            input = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) ? "mouse" : "";
        }

        if (input == "")
        {
            return;
        }
        
        timelineManager.Change("Lobby");
    }
}