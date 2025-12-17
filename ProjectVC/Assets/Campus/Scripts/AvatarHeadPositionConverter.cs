using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarHeadPositionConverter : MonoBehaviour, IPunObservable
{

    private PhotonView m_PhotonView;

    public Transform MainCameraTransform;
    public Transform AvatarHeadTransform;

    public void Awake()
    {
        m_PhotonView = GetComponentInParent<PhotonView>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        AvatarHeadTransform.position = Vector3.Lerp(AvatarHeadTransform.position, MainCameraTransform.position, 0.5f);
        AvatarHeadTransform.rotation = Quaternion.Lerp(AvatarHeadTransform.rotation, MainCameraTransform.rotation, 0.5f);
        */
        if (this.m_PhotonView.IsMine) { 

        AvatarHeadTransform.position = Vector3.Lerp(AvatarHeadTransform.position, MainCameraTransform.position, 0.5f);
        AvatarHeadTransform.rotation = Quaternion.Lerp(AvatarHeadTransform.rotation, MainCameraTransform.rotation, 0.5f);
        } else
        {
            Debug.Log("PhotonView is not mine");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();

        if(stream.IsWriting)
        {
            stream.SendNext(AvatarHeadTransform.position);
            stream.SendNext(AvatarHeadTransform.rotation);
        }

    }
}
