using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject Inventory;
    public GameObject playerText;
    // count each player as they come in, inc in rpc
    public int playerCount = 0;
    public int creatorCount = 0;
    // player spawn positions
    private Vector3 player1 =  new Vector3(39f, 6f, 40f);
    private Vector3 player2 =  new Vector3(-35f, 6f, 40f);
    private Vector3 player3 =  new Vector3(43f, 6f, -9f);
    private Vector3 player4 =  new Vector3(43f, 6f, -9f);
    // how many people ahve pressed the ready button, have to prevent later
    int readyCount = 0;
    // keep track of which player selected id
    int playerID = 0;
    // which type of player
    bool isPlayer = false;
    bool isCreator = false;
    bool started = false;
    bool loaded = false;
    bool isReady = false;
    Text players;
    PhotonView pv;

    void Start() {
        lobbyRoom.SetActive(true);
        mainRoom.SetActive(false);
        playerCount = 0;
        creatorCount = 0;
        pv = GetComponent<PhotonView>();
        players = playerText.GetComponent<Text>();
    }

    void Update() {
        if (!started) {
            NumPlayer();
            if (readyCount >= PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient) {
                startButton.SetActive(true);
            }
            else {
                startButton.SetActive(false);
            }
        }
        else if (started && !loaded) {
            LoadWorld();
            // Don't allow people from joining
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
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
        if (isReady) {
            isReady = false;
            pv.RPC("IncReady", RpcTarget.All, false);
        }
        else {
            isReady = true;
            pv.RPC("IncReady", RpcTarget.All, true);
        }
    }

    public void NumPlayer() {
        players.text = readyCount + "/" + PhotonNetwork.CurrentRoom.PlayerCount;
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
            cameraIso.SetActive(true);
            cameraTop.SetActive(false);
            if (playerID == 0) {
            // if (PhotonNetwork.PlayerList.Length == 2) {
                PhotonNetwork.Instantiate(playerPrefab.name, player1, Quaternion.identity);
            }
            else if (playerID == 1) {
            // else if (PhotonNetwork.PlayerList.Length == 3) {
                PhotonNetwork.Instantiate(playerPrefab.name, player2, Quaternion.identity);
            }
            else if (playerID == 2) {
                PhotonNetwork.Instantiate(playerPrefab.name, player3, Quaternion.identity);
            }
            else {
                PhotonNetwork.Instantiate(playerPrefab.name, player4, Quaternion.identity);
            }
        }
        else if (isCreator) {
            loaded = true;
            lobbyRoom.SetActive(false);
            mainRoom.SetActive(true);
            cameraIso.SetActive(false);
            cameraTop.SetActive(true);
            GameObject creator = PhotonNetwork.Instantiate(creatorPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity);
            creator.GetComponent<SpawnObjectAtClick>().Inventory = Inventory;
            creator.GetComponent<SpawnObjectAtClick>().cam = cameraTop.GetComponent<Camera>();
        }
    }
}
