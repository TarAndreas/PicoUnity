/*
 * Activate tasks inside the laboratory by pressing certain keys or initiate a random one by entering a trigger box
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTaskByKeyboard : MonoBehaviour

{

    //public GameObject RoomTrigger;
    public GameObject firstTask;
    public GameObject secondTask;
    public GameObject allTasks;
    //public bool activateTrigger = false;
    public string firstKey = "A";
    public string secondKey = "S";
    public string thirdKey = "D";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) {
            firstTask.SetActive(true);
            Debug.Log("Key A pressed");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            secondTask.SetActive(true);
            Debug.Log("Key S pressed");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            allTasks.SetActive(true);
            Debug.Log("Key D pressed");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!allTasks.activeSelf)
        {
            activateAnTask();
            Debug.Log("Raum A betreten");
        }

    }

    public void activateAnTask()
    {
        int numberRandom = Random.Range(1, 9);

        if (numberRandom >= 5)
        {
            firstTask.SetActive(true);
            allTasks.SetActive(true);
            Debug.Log("Versuch 2 Teil A startet");
        }
        else
        {
            secondTask.SetActive(true);
            allTasks.SetActive(true);
            Debug.Log("Versuch 2 Teil B startet");
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        firstTask.SetActive(false);
        secondTask.SetActive(false);
        allTasks.SetActive(false);
    }
    */
}
