using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLeadShow : MonoBehaviour {

    GameObject myPrefab;

    public void creat()
    {
        Instantiate(myPrefab, transform.position, transform.rotation);
    }
}
