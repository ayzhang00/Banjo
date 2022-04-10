using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    bool isMoving;
    bool isAttacking;
    bool isSoldering;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = GetComponent<CharController>().isMoving;
        isAttacking = GetComponent<CharController>().isAttacking;
        isSoldering = GetComponent<CharController>().isSoldering;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = GetComponent<CharController>().isMoving;
        isAttacking = GetComponent<CharController>().isAttacking;
        isSoldering = GetComponent<CharController>().isSoldering;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isSoldering", isSoldering);
    }
}
