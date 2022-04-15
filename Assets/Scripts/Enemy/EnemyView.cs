using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    EnemyController c;

    // Update is called once per frame
    void Start()
    {
        c = GetComponentInParent<EnemyController>(); 
    }

    // private void OnTriggerStay(Collider other) {
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            c.player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player" && other.gameObject == c.player) {
            c.player = null;
        }
    }
}
