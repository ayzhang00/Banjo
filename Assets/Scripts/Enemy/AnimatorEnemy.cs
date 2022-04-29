using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEnemy : MonoBehaviour
{
    Animator animator;
    EnemyController c;

    void Awake()
    {
        animator = GetComponent<Animator>();
        c = GetComponent<EnemyController>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isWalking", c.isMove);
        animator.SetBool("isAttacking", c.isAttacking);
        animator.SetBool("isHit", c.isHit);
    }
}
