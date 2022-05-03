using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TaskListCreator : MonoBehaviourPun
{
    public Text num;
    PhotonView pv;
    
    // Update is called once per frame
    void Update()
    {
      num.text = (PhotonNetwork.CurrentRoom.PlayerCount - 1).ToString();
    }
}
