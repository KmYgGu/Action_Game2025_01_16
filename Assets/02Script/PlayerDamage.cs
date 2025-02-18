using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PlayerDamage : MonoBehaviour, IDamaged
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
            col.radius = 0.7f;
            col.center = new Vector3(0.0f, 0.5f, 0.0f);
        }
    }


    public void TakeDamage(float damage, GameObject attacker)
    {
        Debug.Log("플레이어 데미지 체크");
    }
}
