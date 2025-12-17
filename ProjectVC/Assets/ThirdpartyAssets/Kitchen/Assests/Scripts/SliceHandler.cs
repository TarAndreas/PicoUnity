using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceHandler : MonoBehaviour
{

    public GameObject[] slices;
    public Game gameInstance;
    //public bool gameHasEnded;

    // Start is called before the first frame update
    void Start()
    {
        var foundGameObj = FindObjectOfType<Game>();
        gameInstance = foundGameObj;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInstance.endGame)
        {
            CleanUpSlices();
        }
    }

    public void getSliceList(GameObject[] varSlices)
    {
        slices = varSlices;
    }

    void CleanUpSlices()
    {


            for (int i = 0; i < slices.Length; i++)
            {

                GameObject pieces = slices[i];
                Destroy(pieces);
                Debug.Log("Deleted piece succesfully");
            }

    }
}
