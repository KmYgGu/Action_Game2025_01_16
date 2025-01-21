using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Vector3 offset;
    private GameObject obj;

    private void Awake()
    {
        obj = GameObject.Find("MainPlayer");
        if(obj != null)
            target = obj.transform;
    }

    // 이 상황에서 왜 LateUpdate를?
    // 캐릭터가 
    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
