using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject LED1;
    public GameObject LED2;
    public GameObject LED3;
    public GameObject VisualChest;
    void Start() {
        VisualChest.SetActive(false);
    }
    void Update() {
        if (LED1.GetComponent<LightUp>().isLitUp &&
            LED2.GetComponent<LightUp>().isLitUp &&
            LED3.GetComponent<LightUp>().isLitUp) {
            VisualChest.SetActive(true);
        }
    }
    
}
