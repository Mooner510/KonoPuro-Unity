using UnityEngine;

namespace _root.Script.UI
{
    public class PressWindow : MonoBehaviour
    {
        private new Animation animation;
        private string input = "";
        private readonly bool OnPress = false;
        private TimelineManager timelineManager;

        private void Awake()
        {
            timelineManager = FindObjectOfType<TimelineManager>();
        }

        private void Update()
        {
            input = Input.inputString;
            if (input == "")
                input = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) ? "mouse" : "";

            if (input == "" || OnPress) return;

            timelineManager.PlayTimeline(timelineManager.stateStack.Peek(), Scenestate.Lobby);
        }
    }
}