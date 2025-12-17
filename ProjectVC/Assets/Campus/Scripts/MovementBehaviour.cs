using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Save the position of the player in a list and create a line along the movement to track user behaviour
/// </summary>

public class MovementBehaviour : MonoBehaviour
{
    float OneSecond;
    public List<Vector3> PostionsList = new List<Vector3>();
    public GameObject ObservedGO;
    public LineRenderer movementLineRenderer;
    //public Transform currentPos;
    string filename = "";
    string movementLogger;

    
    private void OnEnable()
    {
        Application.logMessageReceived += MovementLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= MovementLog;
    }
    
    void Awake()
    {
        filename = Application.dataPath + "/MovementLog.text";
    }

    // Start is called before the first frame update
    void Start()
    {
        OneSecond = 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        OneSecond -= Time.deltaTime;

        if(OneSecond > 0)
        {
            return;
        } else
        {
            TransfromSaveFromObject();
            
        }
    }

    public void TransfromSaveFromObject()
    {


        if (ObservedGO == null)
        {
            
            ObservedGO = GameObject.Find("PlayerBody");
            //Debug.Log("Name: " + ObservedGO.name);
        } else {

            //Debug.Log("Player position -> ");
            //Debug.Log("X: " + ObservedGO.transform.position.x + " Y: " + ObservedGO.transform.position.y + " Z: " + ObservedGO.transform.position.z);
            movementLogger = ObservedGO.transform.position.x + ";" + ObservedGO.transform.position.y + ";" + ObservedGO.transform.position.z;
            //Debug.Log(movementLogger);
                 
            Vector3 currentPos;
            currentPos = ObservedGO.transform.position;

            PostionsList.Add(currentPos);

        }
        OneSecond = 1.5f;
        LineRendererMovement();
        //Application.logMessageReceived += MovementLog;
    }

    public void LineRendererMovement()
    {
        movementLineRenderer.positionCount = PostionsList.Count;
        for (int i = 0; i < PostionsList.Count; i++)
        {
            movementLineRenderer.SetPosition(i, PostionsList[i]);
        }
    }

    public void MovementLog(string logString, string stackTrace, LogType type)
    {
        logString = movementLogger;
        TextWriter tw = new StreamWriter(filename, true);
        tw.WriteLine("[" + System.DateTime.Now + "] " + logString);
        tw.Close();
    }
}
