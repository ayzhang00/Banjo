using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SpawnPlayersUI : MonoBehaviour
{
    public GameObject readyButton;
    public GameObject vButton;
    public GameObject cButton;
    public GameObject lobbyName;

    // player interface
    public GameObject[] p;
    public Sprite[] cSprite;
    public Sprite[] vSprite;
    // whole player object
    Text players;

    Image c;
    Image v;
    
    // Start is called before the first frame update
    void Start()
    {
        c = cButton.GetComponent<Image>();
        v = vButton.GetComponent<Image>();
        lobbyName.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
    }
    public void NonRPCUI(bool isVariant) {
        if (isVariant) {
            v.sprite = vSprite[1];
            c.sprite = cSprite[0];
        }
        else {
            v.sprite = vSprite[0];
            c.sprite = cSprite[1];
        }
        readyButton.SetActive(true);
    }
    [PunRPC]
    public void PlayerUI(int currPlayer, bool isVariant) {
        if (isVariant) {
            p[currPlayer].transform.Find("TitlePlayer").gameObject.SetActive(true);
            p[currPlayer].transform.Find("Enemy").gameObject.SetActive(false);
        }
        else {
            p[currPlayer].transform.Find("TitlePlayer").gameObject.SetActive(false);
            p[currPlayer].transform.Find("Enemy").gameObject.SetActive(true);
        }
    }

    [PunRPC]
    void startPlayer(int currPlayer, string username) {
        p[currPlayer].SetActive(true);
        p[currPlayer].transform.Find("PlayerUser").gameObject.GetComponent<Text>().text = username;
    }
}
