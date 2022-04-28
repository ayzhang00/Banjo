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
    bool recharging = false;
    bool canRecharge = false;
    // CharController c;
    PlayerSounds ps;
    Image loading;
    
    public int loadingNum = 9;
    int currLoading = 0;

    void Start(){
        batteryImage = batteryUI.GetComponent<Image>();
        ps = GetComponent<PlayerSounds>();
        loading = LoadingUI.GetComponent<Image>();
    }

    void Update() {
        HandleUI();
        if (!recharging) {
            UpdateBattery(false);
        }
        if (!c.isDead && c.playing && pv.IsMine && recharging) {
            if (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                GetComponent<CharSolder>().Solder(false);
                solderSound.Stop();
            } 
        }
        if (canRecharge && Input.GetButtonDown("Recharge")) {
            recharging = true;
            ps.chargeSound.Play();
            StartChargeEffects(true);
        }
        if (Input.GetButtonUp("Recharge")) {
            timeRecharged = 0;
            recharging = false;
            if (energy != 4) {
                ps.chargeSound.Stop();
                StartChargeEffects(false);
            } else {
                StartCoroutine(Discharge());
                // StartChargeEffects(false);
            }
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
            StartChargeEffects(false);
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

    private void Recharge() {
        Debug.Log(energy);
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
    private void HandleUI() {
        if (recharging) {
            LoadingUI.SetActive(true);
            CalculateLoading();
            loading.sprite = LoadingSprites[currLoading];
        } else {
            loading.sprite = LoadingSprites[0];
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
    }

    [PunRPC]
    public void StartChargeEffects(bool isActive) {
        transform.Find("ReviveSparks").gameObject.SetActive(isActive);
    }

}
