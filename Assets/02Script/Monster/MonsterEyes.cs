using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class MonsterEyes : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider col;

    private void Awake()
    {
        if (TryGetComponent<Rigidbody>(out rb))
        {
            rb.useGravity = false;
        }
        if (TryGetComponent<SphereCollider>(out col))
        {
            col.isTrigger = true;
            col.radius = 5;
            //
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Àû ¹ß°ß");
            transform.parent.SendMessage("SetTarget", other.gameObject);
        }
    }
}
