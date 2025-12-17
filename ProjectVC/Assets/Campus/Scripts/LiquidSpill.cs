using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activate particle system once GameObject reaches a certain angle on the axis
/// </summary>
public class LiquidSpill : MonoBehaviour
{
    
    [SerializeField]
    public ParticleSystem myParticleSystem;
    
    
    [SerializeField]
    public GameObject myGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(Vector3.down, transform.forward) <= 90f)
        {
            
            myGameObject.SetActive(true);

        }
        else
        {
            
            myGameObject.SetActive(false);
        }
        
    }
}
