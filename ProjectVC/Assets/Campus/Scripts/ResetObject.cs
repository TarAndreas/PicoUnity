using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates selected GameObjects and deletes the old clone. Best use for interactive games, if an GameObjects bugs out.
/// </summary>
public class ResetObject : MonoBehaviour
{

    [SerializeField] public GameObject spawnObject;
    [SerializeField] public GameObject objectToDestroy;


    public void resetObject()
    {
        Instantiate(spawnObject, transform.position, Quaternion.identity);
        Destroy(objectToDestroy);
        changeObjectToDestroy();
    }

    public void changeObjectToDestroy()
    {
        objectToDestroy = spawnObject;
    }
}
