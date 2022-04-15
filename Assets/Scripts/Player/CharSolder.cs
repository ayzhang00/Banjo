using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharSolder : MonoBehaviourPun
{
    public GameObject solder;
    public GameObject SolderUI; 
    public bool canSolder = false;
    public bool isSoldering = false;
    public float timeToCompleteSolder = 2f;
    public float timeSoldered = 0f;
    public bool solderComplete;
    int solderCount = 0;
    CharController c;

    CharEnergy e;
    PhotonView pv; 
    // Start is called before the first frame update
    void Start()
    {
        e = GetComponent<CharEnergy>();
        pv = GetComponent<PhotonView>();
        c = GetComponent<CharController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSolder() {
        // F is solder
        if (Input.GetButtonDown("Solder") && canSolder) {
            Solder(true);
            solderComplete = false;
            c.solderSound.Play();
        }
        if (Input.GetButtonUp("Solder")){ 
            Solder(false);
            c.solderSound.Stop();
        }
        if (isSoldering) {
            timeSoldered += Time.deltaTime;
            if (timeSoldered >= timeToCompleteSolder) {
                solderComplete = true;
                Solder(false);
                solderCount++;
                if (solderCount == 2) {
                    e.DecEnergy();
                    solderCount = 0;
                }
            }
        }
    }

    public void Solder(bool isActive) {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Solder", isActive);
    }

    public void HandleSolderUI() {
        if (canSolder) {
            SolderUI.SetActive(true);
        }
        else {
            SolderUI.SetActive(false);
        }
    }
}
