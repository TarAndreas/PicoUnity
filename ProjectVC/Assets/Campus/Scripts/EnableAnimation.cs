using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimation : MonoBehaviour
{
    public Animator animators;
    

    // Enable animation
    void Start()
    {
        animators.enabled = true;
        
    }

}
