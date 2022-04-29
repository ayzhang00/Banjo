using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAmbience : MonoBehaviour
{
    public AudioSource GeneralAmbience;
    public AudioSource MachineAmbience;
    public AudioSource TreesAmbience;
    public AudioSource CreaksAmbience;

    // Update is called once per frame
    void Update()
    {
        GeneralAmbience.volume = Mathf.Lerp(GeneralAmbience.volume, 0.5f, Time.deltaTime * 2);
        MachineAmbience.volume = Mathf.Lerp(MachineAmbience.volume, 0.5f, Time.deltaTime * 2);
        TreesAmbience.volume = Mathf.Lerp(TreesAmbience.volume, 0.5f, Time.deltaTime * 2);
        CreaksAmbience.volume = Mathf.Lerp(CreaksAmbience.volume, 0.5f, Time.deltaTime * 2);
    }
}
