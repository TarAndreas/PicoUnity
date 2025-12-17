using Assets.Scripts;
using Photon.Pun;
using ReadyPlayerMe;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lighsaber : MonoBehaviour
{
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;
    Transform DestroyGameObject;
    [SerializeField]
    [Tooltip("The blade object")]
    private GameObject _blade = null;
     
    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private int _frameCount;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;
    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;
    PhotonView pv;
    private GameObject[] slices;

    public SliceHandler slhandler;
    //public Game gameInstance;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        //Init mesh and triangles
        _meshParent.transform.position = Vector3.zero;
        _mesh = new Mesh();
        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;

        Material trailMaterial = Instantiate(_meshParent.GetComponent<MeshRenderer>().sharedMaterial);
        trailMaterial.SetColor("Color_8F0C0815", _colour);
        _meshParent.GetComponent<MeshRenderer>().sharedMaterial = trailMaterial;

        Material bladeMaterial = Instantiate(_blade.GetComponent<MeshRenderer>().sharedMaterial);
        bladeMaterial.SetColor("Color_AF2E1BB", _colour);
        _blade.GetComponent<MeshRenderer>().sharedMaterial = bladeMaterial;

        _vertices = new Vector3[_trailFrameLength * NUM_VERTICES];
        _triangles = new int[_vertices.Length];

        //Set starting position for tip and base
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;

        //gameInstance.GetComponentInParent<Game>();
        var foundGameObj = FindObjectOfType<SliceHandler>();
        slhandler = foundGameObj;
    }
    
    void LateUpdate()
    {
        //Reset the frame count one we reach the frame length
        if(_frameCount == (_trailFrameLength * NUM_VERTICES))
        {
            _frameCount = 0;
        }

        //Draw first triangle vertices for back and front
        _vertices[_frameCount] = _base.transform.position;
        _vertices[_frameCount + 1] = _tip.transform.position;
        _vertices[_frameCount + 2] = _previousTipPosition;
        _vertices[_frameCount + 3] = _base.transform.position;
        _vertices[_frameCount + 4] = _previousTipPosition;
        _vertices[_frameCount + 5] = _tip.transform.position;

        //Draw fill in triangle vertices
        _vertices[_frameCount + 6] = _previousTipPosition;
        _vertices[_frameCount + 7] = _base.transform.position;
        _vertices[_frameCount + 8] = _previousBasePosition;
        _vertices[_frameCount + 9] = _previousTipPosition;
        _vertices[_frameCount + 10] = _previousBasePosition;
        _vertices[_frameCount + 11] = _base.transform.position;

        //Set triangles
        _triangles[_frameCount] = _frameCount;
        _triangles[_frameCount + 1] = _frameCount + 1;
        _triangles[_frameCount + 2] = _frameCount + 2;
        _triangles[_frameCount + 3] = _frameCount + 3;
        _triangles[_frameCount + 4] = _frameCount + 4;
        _triangles[_frameCount + 5] = _frameCount + 5;
        _triangles[_frameCount + 6] = _frameCount + 6;
        _triangles[_frameCount + 7] = _frameCount + 7;
        _triangles[_frameCount + 8] = _frameCount + 8;
        _triangles[_frameCount + 9] = _frameCount + 9;
        _triangles[_frameCount + 10] = _frameCount + 10;
        _triangles[_frameCount + 11] = _frameCount + 11;

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;

        //Track the previous base and tip positions for the next frame
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
        _frameCount += NUM_VERTICES;

        //pv.RPC("CleanUpSlices", RpcTarget.AllBuffered);
        //slhandler.getSliceList(slices);
    }

    private void OnTriggerEnter(Collider other)
    {
        _triggerEnterTipPosition = _tip.transform.position;
        _triggerEnterBasePosition = _base.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        Sliceable sliceable = other.GetComponent<Sliceable>();
        if (sliceable == null)
        {
            Console.WriteLine("ist Nich Sliceble");
        }
        else
        {
            _triggerExitTipPosition = _tip.transform.position;

        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }

        /*GameObject[] */slices = Slicer.Slice(plane, other.gameObject);
        Destroy(other.gameObject);

        Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
        Vector3 newNormal = transformedNormal; //+ Vector3.up * _forceAppliedToCut;
        rigidbody.AddForce(newNormal, ForceMode.Impulse);
        pv.RPC("SliceShow", RpcTarget.AllBuffered);
        pv.RPC("Slicepossion", RpcTarget.AllBuffered);
        slhandler.getSliceList(slices);
            //pv.RPC("CleanUpSlices", RpcTarget.AllBuffered);

        }
   }
    [PunRPC]
    void SliceShow()
    {
        foreach (var slice in slices)
        {

            slice.AddComponent<PhotonView>();
            slice.AddComponent<Rigidbody>();
            slice.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            slice.AddComponent<PhotonRigidbodyView>();
            slice.AddComponent<OwnerTakeOver>();
            slice.AddComponent<Outline>();
            slice.AddComponent<SimpleOutliner>();
            slice.AddComponent<PhotonTransformView>();
            slice.layer = 9;
            slice.tag = "Zutat";
            pv.RPC("Slicepossion", RpcTarget.AllBuffered);
                    
        }
    }
    [PunRPC]
    void Slicepossion()
    {
        foreach (var slice in slices)
        {
            slice.transform.position = slice.transform.position;
            slice.transform.rotation = slice.transform.rotation;
            slice.transform.localScale = slice.transform.localScale;
            slice.transform.localPosition = slice.transform.localPosition;
            
            var alleGameobjects = GameObject.FindGameObjectsWithTag("Destroyable");
            foreach (GameObject gameObject in alleGameobjects)
            {
                if (gameObject.name == "DestroyGameObject" || gameObject.name == "DestroyGameObject(Clone)")
                {
                    DestroyGameObject = gameObject.transform;
                }
            }

            slice.transform.parent = DestroyGameObject;
        }

        //pv.RPC("CleanUpSlices", RpcTarget.AllBuffered);
    }
    /*
    [PunRPC]
    void CleanUpSlices()
    {
        if(gameInstance.endGame) {
            
            for (int i = 0; i < slices.Length; i++)
            {

                GameObject pieces = slices[i];
                Destroy(pieces);
                Debug.Log("Deleted piece succesfully");
            }
        } else
        {
            return;
        }
        
    }
    */
    /*
    public void SliceCleaner()
    {

        if (slices.Length >= 1)
        {

            for (int i = 1; i < slices.Length; i++)
            {

                GameObject pieces = slices[i];
                Destroy(pieces);
                //GameObject[] piece = Slicer.Slice(newplane, other.GameObject);
                Debug.Log("Deleted piece succesfully");
            }
        }
        else
        {
            return;
        }
    }
    */

}
