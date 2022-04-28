using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSounds : MonoBehaviour
{
    // public AudioSource swing;
    // public AudioSource hit;
    // public AudioSource solderSound;
    // public AudioSource chargeSound;
    public AudioSource bg;
    public AudioSource generalSFX;
    public AudioSource solderSound;
    public AudioSource chargeSound;
    public AudioSource reviveSound;

    public AudioClip swing;
    public AudioClip hit;

    // public AudioClip walk;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip walk3;
    public AudioClip walk4;

    // public AudioSource bg;
    public AudioClip bgtrack1;
    public AudioClip bgtrack2;
    // public AudioClip coreTrack2;

    public AudioClip explosion;
    public AudioClip coreTrack;
    bool corePlaying = false;

    GameObject[] LEDs;
    bool allLEDsOff = false;
    
    CharController c;
    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        c = GetComponent<CharController>();
        pv = GetComponent<PhotonView>();
        StartCoroutine("PlayBackgroundMusic");
    }

    // Update is called once per frame
    void Update()
    {
        // if (!c.isDead && c.playing && pv.IsMine) {
        //     if (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
        //         GetComponent<CharSolder>().Solder(false);
        //         solderSound.Stop();
        //         GetComponent<CharEnergy>().StartChargeEffects(false);
        //         chargeSound.Stop();
        //         GetComponent<CharRevive>().StartReviveEffects(false);
        //         reviveSound.Stop();
        //     } 
        // }
        allLEDsOff = true;
        foreach(GameObject LED in LEDs) {
            if (LED.activeSelf) {
                allLEDsOff = false;
            }
        }

        if (allLEDsOff && !corePlaying) {
            bg.Stop();
            StopCoroutine("PlayBackgroundMusic");
            // StartCoroutine("PlayRunToTheCoreMusic");
            PlayRunToTheCoreMusic();
            // explosion.Play();
            generalSFX.PlayOneShot(explosion);
            corePlaying = true;
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Attack") {
            // hit.Play();
            generalSFX.PlayOneShot(hit);
        }
    }

    void PlaySwingSound() {
        // swing.Play();
        generalSFX.PlayOneShot(swing);
    }

    void PlayWalkSound() {
        if (c.canJump) {
            int clip = Random.Range(0, 3);
            switch (clip) {
                case 0:
                    // walk.clip = walk1;
                    generalSFX.PlayOneShot(walk1, 0.7f);
                    break;
                case 1:
                    // walk.clip = walk2;
                    generalSFX.PlayOneShot(walk2, 0.7f);
                    break;
                case 2:
                    // walk.clip = walk3;
                    generalSFX.PlayOneShot(walk3, 0.7f);
                    break;
                case 3:
                    // walk.clip = walk4;
                    generalSFX.PlayOneShot(walk4, 0.7f);
                    break;
            }
            // walk.Play();
        }
    }

    IEnumerator PlayBackgroundMusic() {
        bg.clip = bgtrack1;
        bg.Play();
        yield return new WaitForSeconds(bgtrack1.length);
        bg.clip = bgtrack2;
        bg.Play();
    }

    void PlayRunToTheCoreMusic() {
        bg.clip = coreTrack;
        bg.Play();
    }
}
