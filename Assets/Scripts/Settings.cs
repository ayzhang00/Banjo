using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject Credits;
    public GameObject Audio;
    public GameObject Controls;
    public GameObject General;
    // Start is called before the first frame update
    void OnEnable()
    {
        General.SetActive(true);
        Credits.SetActive(false);
        Audio.SetActive(false);
        Controls.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }
    public void CreditsPressed() {
        Credits.SetActive(true);
        General.SetActive(false);
    }
    public void AudioPressed() {
        Audio.SetActive(true);
        General.SetActive(false);
    }
    public void ControlsPressed() {
        Controls.SetActive(true);
        General.SetActive(false);
    }
    public void LeavePressed() {
        General.SetActive(true);
        Credits.SetActive(false);
        Audio.SetActive(false);
        Controls.SetActive(false);
    }
    public void ContinuePressed() {
        this.gameObject.SetActive(false);
    }
    public void QuitPressed() {
        Application.Quit();
    }
    public void SettingPressed() {
        this.gameObject.SetActive(true);
    }
}
