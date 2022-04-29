using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class CharRevive : MonoBehaviourPun
{
    public GameObject solder;
    public GameObject LoadingUI;
    public Sprite[] LoadingSprites;
    bool canRevive = false;
    bool canRevivePre = false;
    float timeRevived = 0f;
    public float totalTime = 5f;
    public int loadingNum = 9;
    public bool reviving = false;
    int currLoading = 0;
    PhotonView pv;
    Image loading;
    CharController c;
    CharController cOther;
    // PhotonView vOther;
    PlayerSounds ps;
    CharEnergy e;
    CharSolder s;
    GameObject otherPlayer;
    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<CharController>();
        e = GetComponent<CharEnergy>();
        s = GetComponent<CharSolder>();
        ps = GetComponent<PlayerSounds>();
        loading = LoadingUI.GetComponent<Image>();
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!c.isDead && c.playing && pv.IsMine && reviving) {
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire")
                    || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                StartReviveEffects(false);
                // ps.reviveSound.Stop();
                ps.generalSFX.Stop();
                reviving = false;
                currLoading = 0;
            } 
        }

        if (c.playing && pv.IsMine) {
            CanRevive();
            // if (!s.isSoldering && !e.recharging && canRevive) HandleUI();
            HandleUI();
            if (canRevive && Input.GetButtonDown("Revive")) {
                // reviveSparks
                // ps.reviveSound.Play();
                ps.generalSFX.clip = ps.reviveSound;
                ps.generalSFX.Play();
                StartReviveEffects(true);
                reviving = true;
            }
            if (Input.GetButtonUp("Revive")) {
                if (reviving) {
                    // ps.reviveSound.Stop();
                    ps.generalSFX.Stop();
                    StartReviveEffects(false);
                } else {
                    StartCoroutine(FadeSparks());
                }
                reviving = false;
                currLoading = 0;
            }
            if (reviving) {
                Revive();
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Revive") {
            cOther = other.GetComponentInParent<CharController>();
            // vOther = other.GetComponentInParent<PhotonView>();
            otherPlayer = other.gameObject;
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
            if (!ps.corePlaying) {
                e.DecEnergy();
                e.DecEnergy();
            }
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

    IEnumerator FadeSparks() {
        yield return new WaitForSeconds(2f);
        StartReviveEffects(false);
    }

    [PunRPC]
    public void SetRevive() {
        cOther.isRevived = true;
        cOther.isDead = false;
        cOther.isHit = false;
        cOther.health = 15f;
    }

    [PunRPC]
    public void StartReviveEffects(bool isActive) {
        if (otherPlayer) {
            otherPlayer.transform.Find("ReviveSparks").gameObject.SetActive(isActive);
            // pv.RPC("SwitchActiveObject", RpcTarget.All, "Solder", isActive);
            solder.SetActive(isActive);
        }
    }
}
