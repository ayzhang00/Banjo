using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class UsernameJoin : MonoBehaviourPunCallbacks
{
    public InputField username;
    public GameObject enter;
    public Sprite disableEnter;
    public Sprite enableEnter;
    Image enterImage;
    void Start() {
        enterImage = enter.GetComponent<Image>();
    }
    void Update() {
        if (username.text.Length > 0) {
            enterImage.sprite = enableEnter;
        }
        else enterImage.sprite = disableEnter;
    }
    public void EnterRoom() {
        if (username.text.Length > 0) {
            PhotonNetwork.LocalPlayer.NickName = username.text;
            SceneManager.LoadScene("CreateJoin");
        }
    }
    
}
