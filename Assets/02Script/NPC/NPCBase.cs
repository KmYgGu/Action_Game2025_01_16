using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class NPCBase : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rb;

    [SerializeField] private GameObject popupObj;

    private bool isOn = false;

    private void Awake()
    {
        if(TryGetComponent<SphereCollider>(out col))
        {
            col.isTrigger = true;
            col.radius = 2.5f;
        }

        if(TryGetComponent<Rigidbody>(out rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true; // 외부의 힘에 의한 영향을 받지 않는다
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isOn && other.CompareTag("Player"))
        {
            isOn = true;
            if(popupObj.TryGetComponent<IPopUp>(out IPopUp pop))
            {
                pop.PopupOpen();
            } 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isOn && other.CompareTag("Player"))
        {
            isOn = false;
            if(popupObj.TryGetComponent<IPopUp>(out IPopUp pop))
                pop.PopupClose();
            
        }
    }
}
