using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CHECK USB
 */
public class ObjectSizeTrigger : MonoBehaviour
{
    private Vector3 sizeVector;




    // Start is called before the first frame update
    void Start()
    {
        sizeVector = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zutat")
        {
            //GetComponent<Collider>().bounds.size;
        } else if(GetComponent<Collider>().bounds.size != sizeVector)
        {
            Destroy(other.gameObject);
        }
    }


}
