using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    GameObject target;
    void Start() {
        target = transform.parent.gameObject.transform.parent.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position + Vector3.up * 5f);
        
    }
}
