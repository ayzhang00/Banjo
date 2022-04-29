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
    public AudioSource treesAmbience;
    public AudioSource creaksAmbience;
    public AudioSource generalSFX;
    // public AudioSource solderSound;
    // public AudioSource chargeSound;
    // public AudioSource reviveSound;

    public AudioClip solderSound;
    public AudioClip chargeSound;
    public AudioClip reviveSound;

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
    public bool corePlaying = false;

    GameObject[] LEDs;
    bool allLEDsOff = false;

    bool onGrass = false;
    
    CharController c;
    CharEnergy e;
    PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        Debug.Log("LEDs: " + LEDs.Length);
        foreach(GameObject LED in LEDs) {
            Debug.Log("LED: " + LED.transform.parent.transform.position + " " + LED.transform.parent.name);
        }


        c = GetComponent<CharController>();
        e = GetComponent<CharEnergy>();
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
        // Debug.Log(allLEDsOff);

        if (allLEDsOff && !corePlaying) {
            bg.Stop();
            StopCoroutine("PlayBackgroundMusic");
            // StartCoroutine("PlayRunToTheCoreMusic");
            PlayRunToTheCoreMusic();
            // explosion.Play();
            generalSFX.PlayOneShot(explosion, 1.0f);
            corePlaying = true;
            e.Recharge();
        }

        if (onGrass) {
            treesAmbience.volume = Mathf.Lerp(treesAmbience.volume, 0.6f, Time.deltaTime * 2);
            creaksAmbience.volume = Mathf.Lerp(creaksAmbience.volume, 0.6f, Time.deltaTime * 2);
        } else {
            creaksAmbience.volume = Mathf.Lerp(creaksAmbience.volume, 0f, Time.deltaTime * 2);
            treesAmbience.volume = Mathf.Lerp(treesAmbience.volume, 0f, Time.deltaTime * 2);
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Attack") {
            // hit.Play();
            generalSFX.PlayOneShot(hit);
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            onGrass = false;
        } else if (collision.gameObject.tag == "Grass") {
            onGrass = true;
        }


        // if (collision.gameObject.tag == "Ground" && onGrass) {
        //     // StartCoroutine(FadeInTreeAmbience(false));
        //     onGrass = false;
        //     StopCoroutine("FadeInTreeAmbience");
        //     StartCoroutine("FadeInTreeAmbience", false);
        // } else if (collision.gameObject.tag == "Grass" && !onGrass) {
        //     // StartCoroutine(FadeInTreeAmbience(true));
        //     onGrass = true;
        //     StopCoroutine("FadeInTreeAmbience");
        //     StartCoroutine("FadeInTreeAmbience", true);
        // // } else if (collision.gameObject.tag == "Ground") {
        // //     StartCoroutine(FadeInTreeAmbience(false));
        // }
    }

    // void OnCollisionExit(Collision collision) {
    //     if (collision.gameObject.tag == "Grass") {
    //         // StartCoroutine(FadeInTreeAmbience(false));
    //         StopCoroutine("FadeInTreeAmbience");
    //         StartCoroutine("FadeInTreeAmbience", false);
    //     }
    // }

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
                    generalSFX.PlayOneShot(walk1, 0.6f);
                    break;
                case 1:
                    // walk.clip = walk2;
                    generalSFX.PlayOneShot(walk2, 0.6f);
                    break;
                case 2:
                    // walk.clip = walk3;
                    generalSFX.PlayOneShot(walk3, 0.6f);
                    break;
                case 3:
                    // walk.clip = walk4;
                    generalSFX.PlayOneShot(walk4, 0.6f);
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

    IEnumerator FadeInTreeAmbience(bool fadeIn) {
        if (fadeIn) {
            // treesAmbience.volume = 0;
            // creaksAmbience.volume = 0;
            // treesAmbience.Play();
            while (treesAmbience.volume < 0.5f) {
                treesAmbience.volume += 0.01f;
                creaksAmbience.volume += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
        } else {
            while (treesAmbience.volume > 0f) {
                treesAmbience.volume -= 0.01f;
                creaksAmbience.volume -= 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            // treesAmbience.Stop();
        }
    }

    void PlayRunToTheCoreMusic() {
        bg.clip = coreTrack;
        bg.Play();
    }
}// 
