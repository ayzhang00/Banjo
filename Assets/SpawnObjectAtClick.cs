using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnObjectAtClick : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Camera cam;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                Transform objectHit = hit.transform;

                if (objectHit.tag == "Ground") {
                    // Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                    PhotonNetwork.Instantiate(objectToSpawn.name, hit.point, Quaternion.identity, 0);
                    // PhotonNetwork.Instantiate("Chest", new Vector3(24f, 2f, 20f), Quaternion.identity, 0);
                }
            }
        }
    }
}
