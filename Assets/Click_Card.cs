using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Card : MonoBehaviour
{

    public float Pos_x;
    public float Pos_y;
    public float Pos_z;

    public bool onClick = false;  // 點擊
    // Use this for initialization
    void Start()
    {
        Pos_x = GetComponent<Transform>().position.x;
        Pos_y = GetComponent<Transform>().position.y;
        Pos_z = GetComponent<Transform>().position.z;

    }

    // Update is called once per frame
     void OnMouseDown()
     {

        if (!onClick)
        {
            transform.position = new Vector3(Pos_x, -3.9f, Pos_z);
            onClick = true;
        }
        else if (onClick)
        {
            transform.position = new Vector3(Pos_x, -4.200394f, Pos_z);
            onClick = false;
        }


    }



}
