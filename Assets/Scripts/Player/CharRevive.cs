using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class CharRevive : MonoBehaviourPun
{
    public GameObject LoadingUI;
    public Sprite[] LoadingSprites;
    bool canRevive = false;
    bool canRevivePre = false;
    float timeRevived = 0f;
    public float totalTime = 5f;
    public int loadingNum = 9;
    bool reviving = false;
    int currLoading = 0;
    PhotonView pv;
    Image loading;
    CharController c;
    CharController cOther;
    CharEnergy e;
    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<CharController>();
        e = GetComponent<CharEnergy>();
        loading = LoadingUI.GetComponent<Image>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleUI();
        CanRevive();
        if (canRevive && Input.GetButtonDown("Revive")) {
            reviving = true;
        }
        if (Input.GetButtonUp("Revive")) {
            reviving = false;
        }
        if (reviving) {
            Revive();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Revive") {
            cOther = other.GetComponentInParent<CharController>();
            if (cOther.isDead && canRevivePre) {
                canRevive = true;
            }
        }
    }

    // Q is revive, cannot revive without 2 energy
    private void Revive() {
        timeRevived += Time.deltaTime;
        if (timeRevived >= totalTime) {
            reviving = false;
            pv.RPC("SetRevive", RpcTarget.All);
            e.DecEnergy();
            e.DecEnergy();
        }
    }
    private void HandleUI() {
        if (reviving) {
            LoadingUI.SetActive(true);
            CalculateLoading();
            loading.sprite = LoadingSprites[currLoading];
        }
    }
    private void CalculateLoading() {
        float inc = totalTime / loadingNum;
        if (timeRevived >= ((currLoading + 1)*inc) && currLoading < 9) {
            currLoading++;
        }
    }
    private void CanRevive() {
        if (c.playing && !c.isDead && e.energy > 2) {
            canRevivePre = true;
        }
    }
    [PunRPC]
    public void SetRevive() {
        cOther.isRevived = true;
        cOther.isDead = false;
        cOther.isHit = false;
        cOther.health = 10f;
    }
}
