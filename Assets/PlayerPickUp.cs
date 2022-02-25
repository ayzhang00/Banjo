using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    GameObject p = null;
    bool holding = false;
    bool isPlayer1;
    string pickup;

    void Start() {
        // GameObject parent = transform.parent;
        CharController c = GetComponentInParent<CharController>();
        isPlayer1 = c.isPlayer1;
        
        pickup = isPlayer1 ? "PickUp1" : "PickUp2";
    }
    // Update is called once per frame
    void Update()
    {
        // if (playing) {
            if (Input.GetButtonDown(pickup) && p != null) {
                if (holding) {
                    p.GetComponent<Rigidbody>().isKinematic = false;
                    p.GetComponent<Rigidbody>().detectCollisions = true;
                    p.transform.parent = null;
                } else {
                    p.GetComponent<Rigidbody>().isKinematic = true;
                    p.GetComponent<Rigidbody>().detectCollisions = false;
                    p.transform.position = transform.position;
                    p.transform.parent = transform.parent.transform;
                }
                holding = !holding;
            }
        // } 
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Pickup") {
            p = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (!holding) {
            if (collider.gameObject.tag == "Pickup") {
                p = null;
            }
        }
    }
}
