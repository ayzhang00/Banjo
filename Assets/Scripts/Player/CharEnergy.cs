using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class CharEnergy : MonoBehaviour
{   
    public GameObject LoadingUI;
    public Sprite[] LoadingSprites;
    public int energy = 4;
    public GameObject batteryUI;
    public GameObject RechargeNotif;
    public Sprite[] batterySprite;
    public float totalTime = 5f;
    float timeRecharged = 0f;
    Image batteryImage;
    public bool recharging = false;
    public bool doneDischarging = false;
    bool canRecharge = false;
    CharController c;
    CharSolder s;
    CharRevive r;
    PlayerSounds ps;
    // CreatorMusic cm;
    PhotonView pv; 
    Image loading;
    
    public int loadingNum = 9;
    int currLoading = 0;

    void Start(){
        batteryImage = batteryUI.GetComponent<Image>();
        c = GetComponent<CharController>();
        s = GetComponent<CharSolder>();
        r = GetComponent<CharRevive>();
        ps = GetComponent<PlayerSounds>();
        // cm = GameObject.FindGameObjectsWithTag("Creator")[0].GetComponent<CreatorMusic>();
        pv = GetComponent<PhotonView>();
        loading = LoadingUI.GetComponent<Image>();
    }

    void Update() {
        // if (!s.isSoldering && !r.reviving && canRecharge) HandleUI();
        if (!c.isDead && c.playing && pv.IsMine && recharging) {
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire")
                    || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                StartChargeEffects(false);
                // ps.chargeSound.Stop();
                ps.generalSFX.Stop();
                recharging = false;
                currLoading = 0;
            } 
        }

        if (c.playing && pv.IsMine) {
            HandleUI();
            if (!recharging) {
                UpdateBattery(false);
            }
            if (canRecharge && Input.GetButtonDown("Recharge")) {
                recharging = true;
                // ps.chargeSound.Play();
                ps.generalSFX.clip = ps.chargeSound;
                ps.generalSFX.Play();
                StartChargeEffects(true);
                // cm.chargeSound.Stop();
                // cm.chargeSound.Play();
            }
            if (Input.GetButtonUp("Recharge")) {
                timeRecharged = 0;
                if (energy != 4) {
                    // ps.chargeSound.Stop();
                    ps.generalSFX.Stop();
                    StartChargeEffects(false);
                    // recharging = false;
                } else {
                    StartCoroutine(Discharge());
                    // StartChargeEffects(false);
                }
                recharging = false;
                currLoading = 0;
                // c.chargeSound.Stop();
            }

            if (canRecharge && !recharging && energy < 4) {
                DisplayRecharge(true);
            }
            else {
                DisplayRecharge(false);
            }

            if (recharging) {
                Recharge();
                // c.chargeSound.Play();
            } else {
                // c.chargeSound.Stop();
            }

        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Recharge" && energy < 4) {
            canRecharge = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Recharge") {
            timeRecharged = 0;
            canRecharge = false;
            recharging = false;
        }
    }

    private void UpdateBattery(bool isRecharging) {
        if (isRecharging) {
            // batteryImage.sprite = batterySprite[0];
            batteryImage.sprite = batterySprite[5];
            // StartChargeEffects(false);
        }
        else {
            // batteryImage.sprite = batterySprite[5];
            batteryImage.sprite = batterySprite[energy + 1];
        }
    }

    public void DecEnergy() {
        if (energy > 0) {
            energy--;
        }
    }

    private void DisplayRecharge(bool r) {
        RechargeNotif.SetActive(r);
    }

    public void Recharge() {
        if (ps.corePlaying) {
            energy = 4;
        } else {
            if (energy == 0) UpdateBattery(true);
            else UpdateBattery(false);
            timeRecharged += Time.deltaTime;
            if (timeRecharged >= 5.0f && energy < 4) {
                timeRecharged = 0;
                energy = 4;
                recharging = false;
                currLoading = 0;
            }
            if (energy >= 4) {
                canRecharge = false;
            }
        }
    }

    private void HandleUI() {
        if (recharging) {
            LoadingUI.SetActive(true);
            CalculateLoading();
            loading.sprite = LoadingSprites[currLoading];
        // } else {
        //     loading.sprite = LoadingSprites[0];
        }
    }
    private void CalculateLoading() {
        float inc = totalTime / loadingNum;
        if (timeRecharged >= ((currLoading + 1)*inc) && currLoading < 9) {
            currLoading++;
        }
    }

    IEnumerator Discharge() {
        yield return new WaitForSeconds(2f);
        StartChargeEffects(false);
        recharging = false;
    }

    [PunRPC]
    public void StartChargeEffects(bool isActive) {
        transform.Find("ReviveSparks").gameObject.SetActive(isActive);
    }

}
