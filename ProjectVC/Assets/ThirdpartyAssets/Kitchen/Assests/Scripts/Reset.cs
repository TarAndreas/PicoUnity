using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using UnityEngine.TextCore.Text;
using System.Linq;

public class Reset : MonoBehaviour
{
    [SerializeField] public GameObject Newkitchen;
    [SerializeField] public GameObject oldKitchen;
    [SerializeField] GameObject DestroyGameObject;
    [SerializeField] public GameObject startbutton;
    [SerializeField] public GameObject Endebutton;
    [SerializeField] public GameObject Resetbutton;
    [SerializeField] public GameObject CanvasStart;
    [SerializeField] public GameObject Knife;
    Transform kitchenTransform;
    private PhotonView pv;
    //[SerializeField] public GameObject start;
    //[SerializeField] public GameObject end;

    private void Start()
    {
       pv = GetComponent<PhotonView>();
    }
    public void knifeMacker()
    {
            pv.RPC("Rpc_knifeMacker", RpcTarget.AllBuffered);      
    }
    [PunRPC]
    public void Rpc_knifeMacker()
    {
        //GameObject knife = PhotonNetwork.Instantiate("Knife", new Vector3(-78.5149994f, 0.846268058f, 27.3539028f), Quaternion.identity);
        GameObject knife = Instantiate(Knife, new Vector3(-78.5149994f, 0.846268058f, 27.3539028f), Quaternion.identity);
        //for (int i = 0; i < knife.transform.childCount; i++)
        //{
        //    GameObject child = knife.transform.GetChild(i).gameObject;
        //    child.layer = knife.gameObject.layer;
        //}
    }
    public void resetKitchen()
    { 
        pv = GetComponent<PhotonView>();
        pv.RPC("Rpc_KitRest", RpcTarget.AllBuffered);              
    }
    
    [PunRPC]
    public void Rpc_KitRest()
    {
        startbutton.SetActive(true);
        Endebutton.SetActive(true);
        Resetbutton.SetActive(false);
        var alleGameobjects = GameObject.FindGameObjectsWithTag("Destroyable");

            foreach (GameObject gameObject in alleGameobjects)
            {
                try
                {                    
                    //PhotonNetwork.Destroy(gameObject);
                    gameObject.SetActive(false);
                }
                catch
                {
                    gameObject.SetActive(false);
                }
            }      
        var alleGameobjects2 = GameObject.FindGameObjectsWithTag("Visilbable");
        foreach (GameObject ass in alleGameobjects2)
        {
            ass.SetActive(false);
            Debug.Log(ass.name);
        }
        if (alleGameobjects.Count() > 0)
        {
            foreach (GameObject gameObject in alleGameobjects)
            {
                try
                {                   
                    if (PhotonNetwork.IsMasterClient)
                    {
                        //PhotonNetwork.Destroy(gameObject);
                        gameObject.SetActive(false);
                    }                      
                }
                catch
                {
                    gameObject.SetActive(false);
                }
            }
 
        }
        else
        {
            PhotonNetwork.Instantiate("DestroyGameObject", new Vector3(-75.5059967f, 0.679808617f, 19.8629704f), Quaternion.identity);
            //PhotonNetwork.Instantiate("Kitchen", new Vector3(-75.5059967f, 0.679808617f, 19.8629704f), Quaternion.identity);
        }
        //if (alleGameobjects.Count() == 0)
        //{
        //    PhotonNetwork.Instantiate("DestroyGameObject", new Vector3(-75.5059967f, 0.679808617f, 19.8629704f), Quaternion.identity);
        //    PhotonNetwork.Instantiate("Kitchen", new Vector3(-75.5059967f, 0.679808617f, 19.8629704f), Quaternion.identity);
        //}
    }
}
