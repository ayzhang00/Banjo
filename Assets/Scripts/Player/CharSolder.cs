using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharSolder : MonoBehaviourPun
{
    public GameObject solder;
    public GameObject SolderUI; 
    public GameObject LoadingUI;
    public Sprite[] LoadingSprites;
    public bool canSolder = false;
    public bool isSoldering = false;
    public float timeToCompleteSolder = 3.3f;
    public float timeSoldered = 0f;
    public bool solderComplete;
    int solderCount = 0;
    float loadingNum = 9f;
    int currLoading = 0;
    CharController c;
    PlayerSounds ps;
    Image loading;

    // CreatorMusic cm;
    CharEnergy e;
    CharRevive r;
    PhotonView pv; 
    // Start is called before the first frame update
    void Start()
    {
        e = GetComponent<CharEnergy>();
        r = GetComponent<CharRevive>();
        c = GetComponent<CharController>();
        pv = GetComponent<PhotonView>();
        ps = GetComponent<PlayerSounds>();
        // cm = GameObject.FindGameObjectsWithTag("Creator")[0].GetComponent<CreatorMusic>();
        loading = LoadingUI.GetComponent<Image>();
    }
    
    public void StartSolder() {
        if (!c.isDead && c.playing && pv.IsMine && isSoldering) {
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire")
                    || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                Solder(false);
                // ps.solderSound.Stop();
                ps.generalSFX.Stop();
                currLoading = 0;
                // solderComplete = false;
            } 
        }
        if (c.playing && pv.IsMine) {
            // F is solder
            if (Input.GetButtonDown("Solder") && canSolder) {
                Solder(true);
                solderComplete = false;
                // ps.solderSound.Play();
                ps.generalSFX.clip = ps.solderSound;
                ps.generalSFX.Play();
                // cm.solderSound.Stop();
                // cm.solderSound.Play();
            }
            if (Input.GetButtonUp("Solder")){
                currLoading = 0; 
                Solder(false);
                if (!solderComplete) {
                    // ps.solderSound.Stop();
                    ps.generalSFX.Stop();
                }
                solderComplete = false;
            }
            if (isSoldering) {
                timeSoldered += Time.deltaTime;
                if (timeSoldered >= timeToCompleteSolder) {
                    currLoading = 0;
                    solderComplete = true;
                    Solder(false);
                    solderCount++;
                    if (solderCount == 2 && !ps.corePlaying) {
                        e.DecEnergy();
                        solderCount = 0;
                    }
                }
            }
        }
    }

    public void Solder(bool isActive) {
        pv.RPC("SwitchActiveObject", RpcTarget.All, "Solder", isActive);
    }

    public void HandleSolderUI() {
        if (pv.IsMine) {
            if (canSolder && !solderComplete) {
                if (isSoldering) {
                    SolderUI.SetActive(false);
                    LoadingUI.SetActive(true);
                    CalculateLoading();
                    loading.sprite = LoadingSprites[currLoading];
                }
                else {
                    LoadingUI.SetActive(false);
                    SolderUI.SetActive(true);
                }
            }
            else {
                LoadingUI.SetActive(false);
                SolderUI.SetActive(false);
            }
        }
    }

    private void CalculateLoading() {
        float inc = timeToCompleteSolder / loadingNum;
        if (timeSoldered >= ((currLoading + 1)*inc) && currLoading < 9) {
            currLoading++;
        }
    }
}
