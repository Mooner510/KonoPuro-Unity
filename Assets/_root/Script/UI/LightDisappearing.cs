using UnityEngine;

namespace _root.Script.UI
{
    public class LightDisappearing : MonoBehaviour
    {
        private void Update()
        {
            GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, 0, 0.1f);
        }
    }
}
