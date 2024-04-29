using UnityEngine;

public class BackButton : MonoBehaviour
{
    private GameObject obj;
    private void Awake()
    {
        obj = transform.GetChild(0).gameObject;
    }

    public void OnOff(bool active)
    {
        obj.SetActive(active);
    }
}
