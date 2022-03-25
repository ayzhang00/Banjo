using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public GameObject creatorPrefab;
    public GameObject lobbyRoom;
    public GameObject mainRoom;
    public GameObject cameraIso;
    public GameObject cameraTop;
    public GameObject errorMsg;
    public GameObject errorButton;

    private int playerCount;
    private int creatorCount;

    private Vector3 player1 =  new Vector3(39f, 6f, 40f);
    private Vector3 player2 =  new Vector3(-35f, 6f, 40f);
    private Vector3 player3 =  new Vector3(43f, 6f, -9f);

    void Start() {
        playerCount = 0;
        creatorCount = 0;
    }

    public void CreatePlayer() {
        lobbyRoom.SetActive(false);
        mainRoom.SetActive(true);
        if (playerCount == 0) {
            PhotonNetwork.Instantiate(playerPrefab.name, player1, Quaternion.identity);
        }
        else if (playerCount == 1) {
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
}
