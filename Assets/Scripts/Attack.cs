using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Vector3 og;
    float attackTime = 0f;
    float totalTime = 0.5f;
    Collider c;
    
    void Start() {
        c = GetComponent<Collider>();
        // transform.position = og;
    }

    void OnEnable() {
        c.transform.localPosition = og;
    }

    void Update()
    {
        attackTime += Time.deltaTime;
        Vector3 pos = c.transform.localPosition;
        pos.y += 3f;
        c.transform.localPosition = pos;
        if (attackTime >= totalTime) {
            attackTime = 0;
            gameObject.SetActive(false);
        }
    }
}
