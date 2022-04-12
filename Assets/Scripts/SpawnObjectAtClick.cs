using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SpawnObjectAtClick : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject Inventory;
    public Camera cam;
    PhotonView pv; 

    void Start()
    {
        pv = GetComponent<PhotonView>();
        cam = GameObject.Find("TopCamera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                Transform objectHit = hit.transform;

                if (objectHit.tag == "Ground" && Inventory.GetComponent<CloneCount>().pickedUp) {
                    // Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                    PhotonNetwork.Instantiate(objectToSpawn.name, hit.point, Quaternion.identity, 0);
                    // PhotonNetwork.Instantiate("Chest", new Vector3(24f, 2f, 20f), Quaternion.identity, 0);
                }
                Inventory.GetComponent<CloneCount>().pickedUp = false;
            }
        }
        GetCount();
    }
    
    void GetCount() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = enemies.Length;
        Inventory.GetComponent<CloneCount>().clonesPlaced = count;
    }
}
