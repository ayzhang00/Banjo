using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LEDSymbol : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;
    public GameObject LED;
    public GameObject Button;
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
            Button.SetActive(false);
        }
        else {
            self.sprite = Off;
            Button.SetActive(true);
        }
    }
}
