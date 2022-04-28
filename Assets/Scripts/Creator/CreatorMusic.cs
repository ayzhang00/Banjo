using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorMusic : MonoBehaviour
{
    GameObject[] LEDs;
    bool allLEDsOff = false;
    public Camera cam;
    public AudioSource bgMusic;
    public AudioClip coreTrack;
    public AudioClip explosion;
    bool corePlaying = false;

    // public AudioClip coreTrack1;
    // public AudioClip coreTrack2;
    // Start is called before the first frame update
    void Start()
    {
        LEDs = GameObject.FindGameObjectsWithTag("LED");
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
    }

    // IEnumerator PlayRunToTheCoreMusic() {
    //     bgMusic.clip = coreTrack1;
    //     bgMusic.Play();
    //     yield return new WaitForSeconds(bgMusic.clip.length);
    //     bgMusic.clip = coreTrack2;
    //     bgMusic.Play();
    // }
}
