using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SolderLeg : MonoBehaviour
{
    CharSolder c;
    public bool isSoldered = false;

    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "SolderArea") {
            c = other.gameObject.GetComponentInParent<CharSolder>();
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.tag == "SolderArea") {
            c = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (c != null && pv.IsMine) {

        if (c != null) {
            if (c.solderComplete) {
                // gameObject.SetActive(true);
                // Debug.Log("yuh");
                // ColoredCircle.GetComponent<MeshRenderer>().material = Soldered;
                pv.RPC("SwitchSolder", RpcTarget.All, true);
            }
        }
    }
    
    [PunRPC]
    void SwitchSolder(bool soldered) {
        isSoldered = soldered;
        // Debug.Log(isSoldered);
        // Debug.Log("done");

    // void SwitchColors() {
    //     // ColoredCircle.GetComponent<MeshRenderer>().material = Soldered;
    //     gameObject.GetComponent<MeshRenderer>().material = Soldered;
    //     // light.SetActive(true);
    //     Light l = light.GetComponent<Light>();
    //     l.enabled = true;
    //     Debug.Log("done");
    }
}
