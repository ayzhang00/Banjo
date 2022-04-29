using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public Text username;
    public AudioSource click;
    // Start is called before the first frame update
    void Start() {
        username.text = PhotonNetwork.LocalPlayer.NickName;
    }
    public void CreateRoom()
    {
        if (createInput.text.Length >= 1) {
            PhotonNetwork.CreateRoom(createInput.text, new Photon.Realtime.RoomOptions(){MaxPlayers = 5});
        }
        
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Main");
    }

    public void ClickSound() {
        click.PlayOneShot(click.clip);
    }
}
