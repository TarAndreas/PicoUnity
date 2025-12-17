using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class InstanceOnClick : MonoBehaviour
{

    public GameObject ObjectToCreate;
    private GameObject ObjectToDestroy;
    public string ObjectName = "";
    public Transform spawnPosition;


    public void LoadInstanceOnClick()
    {

        ObjectToDestroy = GameObject.Find(ObjectName);
        Debug.Log("" + ObjectName);

        Instantiate(ObjectToCreate, spawnPosition.position, quaternion.identity);
    }
}
