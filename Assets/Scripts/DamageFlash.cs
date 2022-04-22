using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable() {
        StartCoroutine(Flash());
    }

    IEnumerator Flash() {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
