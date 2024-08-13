using UnityEngine;
using UnityEngine.EventSystems;

namespace _root.Script.Title
{
    public class FirstCard : MonoBehaviour
    {
        private new Camera camera;
        private GameObject todayCard;

        private void Awake()
        {
            todayCard = GameObject.Find("Today's Card");
        }

        private void Start()
        {
            todayCard.SetActive(false);
            camera = Camera.main;
        }

        private void Update()
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject()) return;
            if (Physics.Raycast(ray, out var hit, 20, 1 << 6)) todayCard.SetActive(true);
        }
    }
}