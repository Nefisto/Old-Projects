using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class animationScript : MonoBehaviour
{




    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
           
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetBool("isLooking", true);
            animator.SetBool("isIdle", false);

        }
       


        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isLooking", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isLooking", false);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isLooking", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isLooking", false);
        }
      
    if (!animator.GetBool("isWalking") && !animator.GetBool("isLooking"))
        {
            animator.SetBool("isIdle", true);

        }


if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) ||
                Input.GetKeyUp(KeyCode.S))
                animator.SetBool("isWalking", false);





    }

}