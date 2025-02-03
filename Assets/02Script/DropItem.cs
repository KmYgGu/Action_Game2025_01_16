using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 몬스터가 사망했을 때.
// 월드에 드랍
// 인벤토리에 추가될 테이블 ID값과 갯수
// 플레이어랑 충돌체크
// 아이템습득처리.

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class DropItem : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rig;
    private bool isDrop;// 드랍되는 연출 중인지
    private Vector3 pos;
    private Transform rotTrans;
    private float DropPosY;
    private float valueA;

    private void Awake()
    {
        TryGetComponent<SphereCollider>(out col);
        TryGetComponent<Rigidbody>(out rig);

        rig.useGravity = true;
        rig.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        col.radius = 0.5f;
        col.isTrigger = true;

        rotTrans = transform.GetChild(0);
        valueA = 0;

        isDrop = true;

    }

    private void Update()
    {
        if (isDrop)// 드랍 연출이 완료가 되었을 때는 위아래 이동 + 제자리 회전
        {
            rotTrans.Rotate(Vector3.up * (90.0f * Time.deltaTime));
            pos = rotTrans.position;
            valueA += Time.deltaTime;
            pos.y = DropPosY +0.3f * Mathf.Sin(valueA);
            rotTrans.position = pos;
        }
    }

    // 충돌체크
}
