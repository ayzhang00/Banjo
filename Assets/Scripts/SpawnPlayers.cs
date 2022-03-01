using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public float x;
    public float y;
    public float z;
    
    // next, do multiple players spawn in different positions
    public void Start()
    {
        Vector3 pos = new Vector3(x, y, z);
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
    }
}
