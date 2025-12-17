
using UnityEngine;
using Photon.Pun;


public class BauStein : MonoBehaviour
{
    // Start is called before the first frame update
    bool isInTriger = true;
    PhotonView pv;
    Vector3 aktulPosition;
    Rigidbody rb;
    private void Start()
    {
        pv = GetComponent<PhotonView>();       
    }
    private void OnTriggerEnter(Collider other)
    {
        BauStein BS = other.GetComponent<BauStein>();
        if ( BS != null && isInTriger)
        {
            isInTriger = false;
            //BS.isInTriger = false;
            //Vector3 newPosition = transform.position;
            //newPosition.y = newPosition.y + 0.18f;
            //Quaternion newRotation = transform.rotation;
            //other.gameObject.transform.position = newPosition;
            //other.gameObject.transform.rotation = newRotation;
            rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                pv.RPC("RPC_KinimeticActicieren", RpcTarget.All);
            }
            //var BX = other.gameObject.GetComponent<BoxCollider>();
            //BX.isTrigger = false;           
        }
        
    }
    
    public void ActiveKinematic()
    {
        pv.RPC("PRCActiveKinematic", RpcTarget.All);
    }
    [PunRPC]
    private void RPC_KinimeticActicieren()
    {
        rb.isKinematic = true;
    }
    [PunRPC]
    public void PRCActiveKinematic()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        isInTriger = true;
    }

}
