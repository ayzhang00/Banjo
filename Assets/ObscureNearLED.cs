using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObscureNearLED : MonoBehaviour
{
    // public GameObject sphere;
    public GameObject sphere;
    // MeshRenderer sphereRenderer;
    GameObject[] LEDs;
    PhotonView pv;
    bool obscured = false;
    // Start is called before the first frame update
    void Start()
    {
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        pv = gameObject.GetComponent<PhotonView>();
        // sphere = transform.Find("Sphere").gameObject;
        // sphereRenderer = sphere.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine) {
            obscured = false;
            foreach(GameObject LED in LEDs) {
                if (Vector3.Distance(transform.position, LED.transform.position) < 20 && !LED.activeSelf) {
                    obscured = true;
                }
            }
            if (obscured){
                pv.RPC("Obscure", RpcTarget.All, obscured);
                // Obscure(obscured);
            } else {
                pv.RPC("Obscure", RpcTarget.All, obscured);
                // Obscure(obscured);
            }
        }
    }

    [PunRPC]
    void Obscure(bool active) {
        // sphereRenderer.enabled = active;
        sphere.SetActive(!active);
    }
}
