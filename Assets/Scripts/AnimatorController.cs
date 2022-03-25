using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = GetComponent<CharController>().isMoving;
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = GetComponent<CharController>().isMoving;
        animator.SetBool("isWalking", isMoving);
    }
}
