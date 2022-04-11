using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SwitchChangeColors : MonoBehaviour
{
    
    CharController c;
    // public Material notSoldered;
    public GameObject ResistorLeg1;
    public GameObject ResistorLeg2;
    public Material On;
    public Material Off;
    // public GameObject ColoredCircle;
    public GameObject led;

    private bool isOn = false;
    SolderLeg r1;
    SolderLeg r2;

    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        r1 = ResistorLeg1.GetComponent<SolderLeg>();
        r2 = ResistorLeg2.GetComponent<SolderLeg>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            isOn = !isOn;
            if (r1.isSoldered && r2.isSoldered) {
                pv.RPC("SwitchColors", RpcTarget.All);
            }
            // c = other.gameObject.GetComponent<CharController>();
        }
    }
    
    // private void OnTriggerExit(Collider other) {
    //     if (other.tag == "Player") {
    //         c = null;
    //     }
    // }

    // Update is called once per frame
    // void Update()
    // {
    //     // if (c != null && pv.IsMine) {
    //     if (isOn)
    //     if (c != null) {
    //         if (c.solderComplete) {
    //             // gameObject.SetActive(true);
    //             // Debug.Log("yuh");
    //             // ColoredCircle.GetComponent<MeshRenderer>().material = Soldered;
    //             pv.RPC("SwitchColors", RpcTarget.All);
    //         }
    //     }
    // }
    
    [PunRPC]
    void SwitchColors() {
        // ColoredCircle.GetComponent<MeshRenderer>().material = Soldered;
        if (isOn) {
            gameObject.GetComponent<MeshRenderer>().material = On;
            led.SetActive(false);
        }
        else {
            gameObject.GetComponent<MeshRenderer>().material = Off;
            led.SetActive(true);
        }
        
    }
}
