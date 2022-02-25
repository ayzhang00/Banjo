using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Vector3 startPosition = new Vector3(-1.5f,1.6f,2.2f);
    float speed = 30;
    
    void Start() {
    }

    void OnEnable() {
        transform.localPosition = startPosition;
    }

    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;
        if (transform.localPosition.x >= 1.5) {
            gameObject.SetActive(false);
        }
    }
}
