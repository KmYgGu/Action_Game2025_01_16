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

    // �� ��Ȳ���� �� LateUpdate��?
    // ĳ���Ͱ� 
    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
