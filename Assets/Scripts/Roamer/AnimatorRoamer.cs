using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRoamer : MonoBehaviour
{
    Animator animator;
    RoamerController c;
    CharSolder s;

    void Awake()
    {
        animator = GetComponent<Animator>();
        c = GetComponent<RoamerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isWalking", c.canMove);
    }
}
