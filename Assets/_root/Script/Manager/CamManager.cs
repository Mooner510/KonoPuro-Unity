using Cinemachine;
using UnityEngine;

namespace _root.Script.Manager
{
    public class CamManager : MonoBehaviour
    {
        private TimelineManager timelineManager;
        private CinemachineVirtualCamera[] vcams;

        private void Awake()
        {
            timelineManager = FindObjectOfType<TimelineManager>();
            vcams = GetComponentsInChildren<CinemachineVirtualCamera>();
        }

        public void SetCam(Scenestate state)
        {
            foreach (var vcam in vcams) vcam.Priority = 0;
            switch (state)
            {
                case Scenestate.Title:
                    vcams[0].Priority = 10;
                    break;
                case Scenestate.Lobby:
                    vcams[1].Priority = 10;
                    break;
                case Scenestate.Obj1:
                    vcams[2].Priority = 10;
                    break;
                case Scenestate.Obj2:
                    vcams[3].Priority = 10;
                    break;
                case Scenestate.Obj3:
                    vcams[4].Priority = 10;
                    break;
            }
        }
    }
}