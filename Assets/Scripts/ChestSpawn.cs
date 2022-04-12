using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChestSpawn : MonoBehaviour
{   
    // could change this to a list of game objects
    // public GameObject LED1;
    // public GameObject LED2;
    // public GameObject LED3;
    // private bool isNotSpawned = true;
    // void Update() {
    //     if (isNotSpawned) {
    //         // chest drops when all leds lit up
    //         // if (LED1.GetComponent<LightUp>().isLitUp &&
    //         //     LED2.GetComponent<LightUp>().isLitUp &&
    //         //     LED3.GetComponent<LightUp>().isLitUp) {
    //         if (LED1.active &&
    //             LED2.active &&
    //             LED3.active) {
    //             if (PhotonNetwork.IsMasterClient) {
    //                 PhotonNetwork.Instantiate("Chest", new Vector3(5f, 5f, 12f), Quaternion.identity, 0);
    //                 isNotSpawned = false;
    //             }
    //         }
    //     }
    // }
    // public GameObject LED1;
    // public GameObject LED2;
    // public GameObject LED3;
    // Light Light1;
    // Light Light2;
    // Light Light3;

    // private bool isNotSpawned = true;

    // void Start() {
    //     Light1 = LED1.GetComponent<Light>();
    //     Light2 = LED2.GetComponent<Light>();
    //     Light3 = LED3.GetComponent<Light>();
    // }
    // void Update() {
    //     if (isNotSpawned) {
    //         // chest drops when all leds lit up
    //         if (Light1.enabled &&
    //             Light2.enabled &&
    //             Light3.enabled) {
    //         // if (LED1.active &&
    //         //     LED2.active &&
    //         //     LED3.active) {
    //             if (PhotonNetwork.IsMasterClient) {
    //                 PhotonNetwork.Instantiate("Chest", new Vector3(5f, 5f, 12f), Quaternion.identity, 0);
    //                 isNotSpawned = false;
    //             }
    //         }
    //     }
    // }
}
