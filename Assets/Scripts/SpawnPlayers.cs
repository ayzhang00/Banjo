using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public GameObject lobbyRoom;
    public GameObject mainRoom;
    public GameObject cameraIso;
    public GameObject cameraTop;
    public float x;
    public float y;
    public float z;

    public void CreatePlayer() {
        lobbyRoom.SetActive(false);
        mainRoom.SetActive(true);
        Vector3 pos = new Vector3(x, y, z);
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
    }

    public void CreateCreator() {
        lobbyRoom.SetActive(false);
        mainRoom.SetActive(true);
        cameraIso.SetActive(false);
        cameraTop.SetActive(true);
    }
}
