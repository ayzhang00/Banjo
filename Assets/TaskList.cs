using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskList : MonoBehaviour
{
    public GameObject num;
    int LEDnum = 5;

    // Update is called once per frame
    // void Update()
    // {
    //     UpdateNum();
    // }
    public void UpdateNum(bool on) {
        if (on) LEDnum++;
        else LEDnum--;
        num.GetComponent<Text>().text = LEDnum.ToString();
    }
}
