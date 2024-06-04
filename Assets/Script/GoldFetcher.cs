using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldFetcher : MonoBehaviour
{
    [SerializeField] private GameObject gachaui;

    private GachaUI _gachaui;

    public int currentgold;
    // Start is called before the first frame update
    void Start()
    {
        _gachaui = gachaui.GetComponent<GachaUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _gachaui.ChangeGoldTxt(currentgold);
    }
}
