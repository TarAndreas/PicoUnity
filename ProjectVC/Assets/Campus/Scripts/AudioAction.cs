/*
 * Play the selected audio source
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAction : MonoBehaviour
{

    public AudioSource audios;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        audios.Play();
    }
}
