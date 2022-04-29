using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SpawnObjectAtClick : MonoBehaviourPun
{
    public GameObject objectToSpawn;
    public GameObject Inventory;
    public AudioSource CreatorSounds;
    public AudioClip Click;
    public Camera cam;
    public bool playing = true;
    PhotonView pv; 
    int spawned = 0;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        playing = true;
        // cam = GameObject.Find("TopCamera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        playing = !GetComponent<PauseMenu>().isPaused;
        if (playing) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                    Transform objectHit = hit.transform;

                    // Debug.Log(objectHit.tag);
                    if ((objectHit.tag == "Ground" || objectHit.tag == "Grass") && Inventory.GetComponent<CloneCount>().pickedUp) {
                        // Instantiate(objectToSpawn, hit.point, Quaternion.identity);
                        PhotonNetwork.Instantiate(objectToSpawn.name, hit.point, Quaternion.identity, 0);
                        spawned++;
                        CreatorSounds.PlayOneShot(Click);
                        // Debug.Log("spawned");
                        // PhotonNetwork.Instantiate("Chest", new Vector3(24f, 2f, 20f), Quaternion.identity, 0);
                    }
                    Inventory.GetComponent<CloneCount>().pickedUp = false;
                }
            }
            GetCount();
        }


    }
    
    void GetCount() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = enemies.Length;
        Inventory.GetComponent<CloneCount>().clonesPlaced = count;
    }

    public void ContinuePressed() {
        playing = true;
    }
}
