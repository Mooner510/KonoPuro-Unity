using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardinrandomRange : MonoBehaviour
{
    public GameObject[] pickupcard;
    
    public int cardindex;

    private int cardclickcount;
    public Vector3 startposition;
    public Vector3 endposition;
    
    // Start is called before the first frame update
    void Start()
    {
        cardindex=Random.Range(1, 4);
        cardclickcount = 0;
        startposition.x = gameObject.transform.position.x;
        startposition.y = gameObject.transform.position.y;
        startposition.z = gameObject.transform.position.z;
        gameObject.transform.position = startposition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (cardclickcount == 0)
        {
            pickupcard[0].SetActive(false);
            pickupcard[cardindex].SetActive(true);
        }
        else if (cardclickcount%2==1)
        {
            transform.gameObject.transform.position = Vector3.Lerp(startposition, endposition, 500f);
        }
        else if(cardclickcount%2==0)
        {
            transform.gameObject.transform.position = Vector3.Lerp(endposition, startposition, 500f);
        }

        cardclickcount++;

    }
}
