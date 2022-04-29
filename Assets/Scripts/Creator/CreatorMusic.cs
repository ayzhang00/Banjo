using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorMusic : MonoBehaviour
{
    GameObject[] LEDs;
    GameObject[] players;
    bool allLEDsOff = false;
    public Camera cam;
    public AudioSource bgMusic;
    public AudioSource solderSound;
    public AudioSource chargeSound;
    public AudioClip coreTrack;
    public AudioClip explosion;

    bool playerSoldering = false;
    bool playerCharging = false;

    bool corePlaying = false;

    // public AudioClip coreTrack1;
    // public AudioClip coreTrack2;
    // Start is called before the first frame update
    void Start()
    {
        LEDs = GameObject.FindGameObjectsWithTag("LED");
        players = GameObject.FindGameObjectsWithTag("Player");
        // bgMusic = GameObject.Find("TopCamera").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        allLEDsOff = true;
        foreach(GameObject LED in LEDs) {
            if (LED.activeSelf) {
                allLEDsOff = false;
            }
        }

        if (allLEDsOff && !corePlaying) {
            bgMusic.Stop();
            // StartCoroutine("PlayRunToTheCoreMusic");
            bgMusic.clip = coreTrack;
            bgMusic.Play();
            bgMusic.PlayOneShot(explosion);
            corePlaying = true;
        }

        playerSoldering = false;
        playerCharging = false;
        foreach(GameObject player in players) {
            if(player.transform.Find("Solder").gameObject.activeSelf) {
                playerSoldering = true;
            }
            if (player.GetComponent<CharEnergy>().recharging) {
                playerCharging = true;
            }
        }
        if (!playerSoldering) {
            solderSound.Stop();
        }
        if (!playerCharging) {
            chargeSound.Stop();
        }

    }

    // IEnumerator PlayRunToTheCoreMusic() {
    //     bgMusic.clip = coreTrack1;
    //     bgMusic.Play();
    //     yield return new WaitForSeconds(bgMusic.clip.length);
    //     bgMusic.clip = coreTrack2;
    //     bgMusic.Play();
    // }
}
