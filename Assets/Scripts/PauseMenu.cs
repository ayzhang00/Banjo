using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPun
{
    public GameObject PauseUI;
    GameObject Credits;
    GameObject Audio;
    GameObject Controls;
    GameObject General;
    public bool isPaused = false;
    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        Credits = PauseUI.transform.Find("Credits").gameObject;
        Audio = PauseUI.transform.Find("Audio").gameObject;
        Controls = PauseUI.transform.Find("Controls").gameObject;
        General = PauseUI.transform.Find("General").gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        // Debug.Log(isPaused);
        if (Input.GetKeyDown(KeyCode.Escape) && pv.IsMine)
        {
            isPaused = !isPaused;
            PauseUI.SetActive(isPaused);
        }
        
    }

    public void LeaveGamePressed() {
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
    public void CreditsPressed() {
        Credits.SetActive(true);
        General.SetActive(false);
    }
    public void AudioPressed() {
        Audio.SetActive(true);
        General.SetActive(false);
    }
    public void ControlsPressed() {
        Controls.SetActive(true);
        General.SetActive(false);
    }
    public void LeavePressed() {
        General.SetActive(true);
        Credits.SetActive(false);
        Audio.SetActive(false);
        Controls.SetActive(false);
    }
    public void QuitPressed() {
        Application.Quit();
    }
    public void SettingPressed() {
        this.gameObject.SetActive(true);
    }
}
