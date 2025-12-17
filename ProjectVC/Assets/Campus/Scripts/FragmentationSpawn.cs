using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentationSpawn : MonoBehaviour
{
    [SerializeField] public GameObject shatteredObject;
    
    /// <summary>
    /// Objects inside the trigger with the tag will instantiate the assigned GameObject
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Floor")
        {
            Instantiate(shatteredObject, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }


}
