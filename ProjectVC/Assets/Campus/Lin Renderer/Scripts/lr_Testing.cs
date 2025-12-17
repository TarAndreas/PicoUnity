using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class lr_Testing : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private lr_lineController line;

    private void Start()
    {
        //line.SetUpLine(points);
        PhotonView pv = PhotonView.Get(this);
        pv.RPC("StartRenderLine", RpcTarget.All);
    }

    [PunRPC]
    public void StartRenderLine()
    {
        line.SetUpLine(points);
    }
}
