using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    public GameObject ConnectOptionsPanelGameobject;
    public GameObject ConnectWithNamePanelGameobject;
    public GameObject ConnectAsAdmin_PanelGameobject;

    // Start is called before the first frame update
    void Start()
    {
        ConnectOptionsPanelGameobject.SetActive(true);
        ConnectWithNamePanelGameobject.SetActive(false);
        ConnectAsAdmin_PanelGameobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
