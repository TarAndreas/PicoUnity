using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class Hand : MonoBehaviour
{

    Animator animator;
    private float triggerTarget;
    private float gripTarget;
    private float gripCurrent;
    private float triggerCurrent;
    [SerializeField] private float speed;
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        AnimateHand();
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }
    void AnimateHand()
    {
        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent,gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGripParam, gripCurrent);

        }
        if (triggerCurrent != triggerTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorTriggerParam, gripCurrent);

        }
    }  

}
