using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    float attackTime = 0f;
    float totalTime = 1f;
    
    void Start() {
    }

    void OnEnable() {
    }

    void Update()
    {
        attackTime += Time.deltaTime;
        if (attackTime >= totalTime) {
            attackTime = 0;
            gameObject.SetActive(false);
        }
    }
}
