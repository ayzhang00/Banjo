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

    public bool isOn = false;
    bool changed = false;
    SolderLeg r1;
    SolderLeg r2;

    public AudioClip switchSound;

    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        r1 = ResistorLeg1.GetComponent<SolderLeg>();
        r2 = ResistorLeg2.GetComponent<SolderLeg>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine) {
            if (r1.isSoldered && r2.isSoldered && !changed) {
                pv.RPC("SwitchColors", RpcTarget.All);
                pv.RPC("SwitchChanged", RpcTarget.All, true);
                // switchSound.Play();
                AudioSource a = other.gameObject.GetComponent<PlayerSounds>().generalSFX;
                a.PlayOneShot(switchSound);
            }
            // c = other.gameObject.GetComponent<CharController>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            pv.RPC("SwitchChanged", RpcTarget.All, false);
        }
    }

    public void ResetLED() {
        Debug.Log("here");
        isOn = true;
        pv.RPC("SwitchColors", RpcTarget.All);
        pv.RPC("SwitchChanged", RpcTarget.All, false);
        r1.pv.RPC("SwitchSolder", RpcTarget.All, false);
        r2.pv.RPC("SwitchSolder", RpcTarget.All, false);
    }
    
    [PunRPC]
    void SwitchColors() {
        if (!isOn) {
            GetComponent<MeshRenderer>().material = On;
            led.SetActive(false);
            isOn = true;
        }
        else {
            GetComponent<MeshRenderer>().material = Off;
            led.SetActive(true);
            isOn = false;
        }        
    }
    [PunRPC]
    void SwitchChanged(bool isChanged) {
        changed = isChanged;
    }
}
