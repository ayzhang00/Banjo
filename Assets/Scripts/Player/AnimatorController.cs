using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    CharController c;

    void Awake()
    {
        animator = GetComponent<Animator>();
        c = GetComponent<CharController>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isWalking", c.isMoving);
        animator.SetBool("isAttacking", c.isAttacking);
        animator.SetBool("isSoldering", c.isSoldering);
        animator.SetBool("isHit", c.isHit);
        animator.SetBool("isDead", !c.playing);
    }
}
