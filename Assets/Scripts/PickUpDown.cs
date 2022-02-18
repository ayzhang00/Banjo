using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDown : MonoBehaviour
{
    public Transform dest;
    bool isPickedUp = false;
    void OnMouseDown()
    {
        if (isPickedUp) {
            GetComponent<Rigidbody>().useGravity = true;
            this.transform.parent = null;
            isPickedUp = false;
        }
        else {
            GetComponent<Rigidbody>().useGravity = false;
            this.transform.position = dest.position;
            this.transform.parent = GameObject.Find("Destination").transform;
            isPickedUp = true;
        }
        
    }
}
