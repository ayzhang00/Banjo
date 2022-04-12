using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LEDSymbol : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;
    public GameObject LED;
    Image self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LED.activeSelf) {
            self.sprite = On;
        }
        else {
            self.sprite = Off;
        }
    }
}
