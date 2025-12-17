using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class TriggerInteractor : MonoBehaviour, IPointerClickHandler
{
    private BoxCollider boxColl;
    private Rigidbody rig;
    public int id;

    private void Start()
    {
        boxColl = GetComponent<BoxCollider>();
        boxColl.size = new Vector3(1, 2, 1);
        boxColl.center = new Vector3(0, 1, 0);
        boxColl.isTrigger = true;
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        rig.isKinematic = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnHover()
    {
    }

    public void OnClick()
    {
    }
}
