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

    private void OnTriggerStay(Collider other) {
    // private void OnTriggerEnter(Collider other) {
        if (other.tag == "ResistorSolder") {
            // cannot solder again once soldered
            if (!other.GetComponent<SolderLeg>().isSoldered){
                c.canSolder = true;
            }
            else {
                c.canSolder = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "ResistorSolder") {
            c.canSolder = false;
        }
    }
}
