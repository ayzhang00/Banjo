using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LightUp : MonoBehaviour
{
    public Material M1;
    public Material M2;
    public bool isLitUp = false;
    private PhotonView pv;

    void Awake(){
        pv = GetComponent<PhotonView>();
    }
    void OnTriggerEnter(Collider collider) {
        if (pv.IsMine) {
            if (collider.tag == "Attack") {
                pv.RPC("ChangeColor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void ChangeColor() {
        if (isLitUp) {
            GetComponent<MeshRenderer>().material = M1;
            isLitUp = false;
        }
        else {
            GetComponent<MeshRenderer>().material = M2;
            isLitUp = true;
        }
    }
}
