using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour
{
    public Material M1;
    public Material M2;
    bool isLitUp = false;
    void OnMouseDown()
    {
        if (isLitUp) {
            GetComponent<MeshRenderer>().material = M1;
            isLitUp = false;
        }
        else {
            GetComponent<MeshRenderer>().material = M2;
            isLitUp = true;
        }
        
    }
}
