using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDown : MonoBehaviour
{
    // public Transform dest;
    bool isPickedUp = false;
    bool p1 = false;
    void PickUp(string dest, bool fromP1)
    {
        if (isPickedUp && ((fromP1 && p1) || !(fromP1 || p1))) {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().detectCollisions = true;
            this.transform.parent = null;
            isPickedUp = false;
        }
        else if (!isPickedUp) {
            p1 = fromP1;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().detectCollisions = false;
            this.transform.position = GameObject.Find(dest).transform.position;
            this.transform.parent = GameObject.Find(dest).transform;
            isPickedUp = true;
        }
        
    }
    void Update() 
    {
        if (Input.GetButtonDown("PickUp1")) {
            PickUp("Destination1", true);
        }
        else if (Input.GetButtonDown("PickUp2")) {
            PickUp("Destination2", false);
        }
    }
}
