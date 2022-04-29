using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class FixLED : MonoBehaviour
{
    public GameObject SwitchController;
    public GameObject LoadingUI;
    public Sprite[] LoadingSprites;
    public float totalTime = 10f;
    int currLoading = 0;
    int loadingNum = 9;
    float timeHeld = 0f;
    bool held = false;
    Image loading;
    SwitchChangeColors s;
    // Start is called before the first frame update
    void Start()
    {
        loading = LoadingUI.GetComponent<Image>();
        s = SwitchController.GetComponent<SwitchChangeColors>();
    }

    // Update is called once per frame
    void Update()
    {
        if(held) {
            OnHold();
        }
    }
    public void OnClick() {
        held = true;
    }
    public void OnHold() {
        HandleUI();
        timeHeld += Time.deltaTime;
        Debug.Log(timeHeld);
        if (timeHeld >= totalTime) {
            s.ResetLED();
            timeHeld = 0;
            currLoading = 0;
            held = false;
            LoadingUI.SetActive(false);
        }
    }
    public void OnUp() {
        held = false;
    }
    private void HandleUI() {
            LoadingUI.SetActive(true);
            CalculateLoading();
            loading.sprite = LoadingSprites[currLoading];
    }
    private void CalculateLoading() {
        float inc = totalTime / loadingNum;
        if (timeHeld >= ((currLoading + 1)*inc) && currLoading < 9) {
            currLoading++;
        }
    }
}
