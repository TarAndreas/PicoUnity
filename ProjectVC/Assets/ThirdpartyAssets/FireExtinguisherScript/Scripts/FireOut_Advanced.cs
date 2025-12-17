/*
    This script is placed on the fire particle.
    It controls the fire extinguishing rate by reducing the emission each time a 'Water' particle collides with it.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOut_Advanced: MonoBehaviour {

    private ParticleSystem fireParticles;
    private ParticleSystem.EmissionModule fireEmission;
    private bool notFound = false;
    private float currentEmission = 0;
    [SerializeField] private float fadeRate = 1;
    [SerializeField] private GameObject gOTurnOff;
    [SerializeField] private GameObject customAudioSource;


    // Use this for initialization
    void Start()
    {
        // Get Particle System and emission
        if (GetComponent<ParticleSystem>())
        {
            fireParticles = GetComponent<ParticleSystem>();
            fireEmission = fireParticles.emission;
            currentEmission = fireEmission.rateOverTime.constant;
        }
        else
        {
            Debug.Log("ERROR :: FireOut - Particle System not found on object: " + gameObject.name);
            notFound = true;
        }

    }
	
	void Update () {
        // if no particle system, do nothing
        if (notFound == true)
        {
            return;
        }

        // check if fire is extinguished
        if (currentEmission <= 0)
        {
            fireParticles.Stop();
            deactivateFunction();
        }
	}

    private void OnParticleCollision(GameObject other)
    {
        // if fire got wet decrease emission rate
        if (other.tag == "Water")
        {
            currentEmission -= fadeRate;
            fireEmission.rateOverTime = currentEmission;
            
        }
    }

    public void deactivateFunction()
    {
        gOTurnOff.SetActive(false);
        customAudioSource.SetActive(false);
    }
}
