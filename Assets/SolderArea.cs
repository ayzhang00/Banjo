using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolderArea : MonoBehaviour
{
    CharController c;

    // Update is called once per frame
    void Start()
    {
        c = GetComponentInParent<CharController>(); 
    }

    // private void OnTriggerStay(Collider other) {
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Switch") {
            c.canSolder = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Switch") {
            c.canSolder = false;
        }
    }
}
