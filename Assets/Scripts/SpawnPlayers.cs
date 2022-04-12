using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SpawnPlayers : MonoBehaviour
{
    // imported objects
    public GameObject playerPrefab;
    public GameObject creatorPrefab;
    public GameObject lobbyRoom;
    public GameObject mainRoom;
    public GameObject cameraIso;
    public GameObject cameraTop;
    public GameObject errorMsg;
    public GameObject errorButton;
    public GameObject readyButton;
    public GameObject startButton;
    // count each player as they come in, inc in rpc
    public int playerCount = 0;
    public int creatorCount = 0;
    // player spawn positions
    private Vector3 player1 =  new Vector3(39f, 6f, 40f);
    private Vector3 player2 =  new Vector3(-35f, 6f, 40f);
    private Vector3 player3 =  new Vector3(43f, 6f, -9f);
    // how many people ahve pressed the ready button, have to prevent later
    int readyCount = 0;
    // keep track of which player selected id
    int playerID = 0;
    // which type of player
    bool isPlayer = false;
    bool isCreator = false;
    bool started = false;
    bool loaded = false;

    PhotonView pv;

    void Start() {
        lobbyRoom.SetActive(true);
        mainRoom.SetActive(false);
        playerCount = 0;
        creatorCount = 0;
        pv = GetComponent<PhotonView>();
    }

    void Update() {
        if (readyCount >= PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient && creatorCount > 0) {
            startButton.SetActive(true);
        }
        else {
            startButton.SetActive(false);
        }
        if (started && !loaded) {
            LoadWorld();
        }
        Debug.Log(readyCount);
        Debug.Log("players:"+PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public void CreatePlayer() {
        playerID = playerCount;
        isPlayer = true;
        pv.RPC("IncPlayers", RpcTarget.All, true);
        // if was creator, remove
        if (isCreator) {
            isCreator = false;
            pv.RPC("IncCreator", RpcTarget.All, false);
        }
        readyButton.SetActive(true);
    }

    public void CreateCreator() {
        if (creatorCount == 0) {
            pv.RPC("IncCreator", RpcTarget.All, true);
            isCreator = true;
            if (isPlayer) {
                isPlayer = false;
                pv.RPC("IncPlayers", RpcTarget.All, false);
            }
            
            readyButton.SetActive(true);
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
    public void Started() {
        pv.RPC("setStart", RpcTarget.All);
    }

    public void ClickReady() {
        pv.RPC("IncReady", RpcTarget.All, true);
    }
    [PunRPC]
    void IncReady(bool up) {
        if (up) readyCount++;
        else readyCount--;
    }

    [PunRPC]
    void IncPlayers(bool up) {
        if (up) playerCount++;
        else playerCount--;
    }

    [PunRPC]
    void IncCreator(bool up) {
        if (up) creatorCount++;
        else creatorCount--;
    }
    // set start for everyone
    [PunRPC]
    void setStart() {
        started = true;
    }

    // load world
    void LoadWorld() {
        if (isPlayer) {
            loaded = true;
            lobbyRoom.SetActive(false);
            mainRoom.SetActive(true);
            if (playerID == 0) {
            // if (PhotonNetwork.PlayerList.Length == 2) {
                PhotonNetwork.Instantiate(playerPrefab.name, player1, Quaternion.identity);
            }
            else if (playerID == 1) {
            // else if (PhotonNetwork.PlayerList.Length == 3) {
                PhotonNetwork.Instantiate(playerPrefab.name, player2, Quaternion.identity);
            }
            else {
                PhotonNetwork.Instantiate(playerPrefab.name, player3, Quaternion.identity);
            }
        }
        else if (isCreator) {
            loaded = true;
            lobbyRoom.SetActive(false);
            mainRoom.SetActive(true);
            cameraIso.SetActive(false);
            cameraTop.SetActive(true);
            PhotonNetwork.Instantiate(creatorPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }
}
