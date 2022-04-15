using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPun
{
    public GameObject PauseUI;
    bool isPaused = false;
    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseUI.SetActive(isPaused);
        }
        
    }

    public void QuitPressed() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Loading");
        // if (pv.IsMine) {
        //     PhotonNetwork.LeaveRoom();
        // }
    }

    // void OnPlayerLeftRoom() {
    //     SceneManager.LoadScene("Loading");
    // }

    public void ContinuePressed() {
        isPaused = false;
        PauseUI.SetActive(isPaused);
    }
}
