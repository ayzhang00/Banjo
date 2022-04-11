using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SpawnPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public GameObject creatorPrefab;
    // Camera topcam;
    // Camera isocam;
    // GameObject[] players;
    // GameObject[] creators;
    public GameObject lobbyRoom;
    public GameObject mainRoom;
    public GameObject cameraIso;
    public GameObject cameraTop;
    public GameObject errorMsg;
    public GameObject errorButton;

    public int playerCount = 0;
    public int creatorCount = 0;

    private Vector3 player1 =  new Vector3(39f, 6f, 40f);
    private Vector3 player2 =  new Vector3(-35f, 6f, 40f);
    private Vector3 player3 =  new Vector3(43f, 6f, -9f);

    int readyCount = 0;

    PhotonView pv;

    void Start() {
        // //If there is no creator, that means this is the first person in the lobby, so make them the creator
        // if (PhotonNetwork.PlayerList.Length == 1) {
        //     CreateCreator();
        //     // topcam.enabled = true;
        //     // isocam.enabled = false;
        //     topcam.SetActive(true);
        //     isocam.SetActive(false);
        // } else {
        //     //For players after the first one, make them normal players
        //     CreatePlayer();
        //     // topcam.enabled = false;
        //     // isocam.enabled = true;
        //     topcam.SetActive(false);
        //     isocam.SetActive(true);
        // }


        // creators = GameObject.FindGameObjectsWithTag("Creator");
        // Debug.Log(creators);
        lobbyRoom.SetActive(true);
        mainRoom.SetActive(false);
        playerCount = 0;
        creatorCount = 0;
        pv = GetComponent<PhotonView>();
    }

    void Update() {
        // if (readyCount == PhotonNetwork.CountOfPlayersInRooms) {
            
        // }
        // Debug.Log(readyCount);
    }

    public void CreatePlayer() {
        lobbyRoom.SetActive(false);
        mainRoom.SetActive(true);
        if (playerCount == 0) {
        // if (PhotonNetwork.PlayerList.Length == 2) {
            PhotonNetwork.Instantiate(playerPrefab.name, player1, Quaternion.identity);
        }
        else if (playerCount == 1) {
        // else if (PhotonNetwork.PlayerList.Length == 3) {
            PhotonNetwork.Instantiate(playerPrefab.name, player2, Quaternion.identity);
        }
        else {
            PhotonNetwork.Instantiate(playerPrefab.name, player3, Quaternion.identity);
        }
        playerCount++;
    }

    public void CreateCreator() {
        if (creatorCount == 0) {
            lobbyRoom.SetActive(false);
            mainRoom.SetActive(true);
            cameraIso.SetActive(false);
            cameraTop.SetActive(true);
            PhotonNetwork.Instantiate(creatorPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
            creatorCount++;
        }
        else{
            errorMsg.SetActive(true);
            errorButton.SetActive(true);
        }
    }
    public void ClearError(){
        errorMsg.SetActive(false);
        errorButton.SetActive(false);
    }

    public void ClickReady() {
        pv.RPC("IncReady", RpcTarget.All);
    }
    [PunRPC]
    void IncReady() {
        readyCount++;
    }
}
