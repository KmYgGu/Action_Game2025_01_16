using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���Ͱ� ������� ��.
// ���忡 ���
// �κ��丮�� �߰��� ���̺� ID���� ����
// �÷��̾�� �浹üũ
// �����۽���ó��.

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class DropItem : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rig;
    private bool isDrop;// ����Ǵ� ���� ������
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

        isDrop = false;

    }

    private void Update()
    {
        if (isDrop)// ��� ������ �Ϸᰡ �Ǿ��� ���� ���Ʒ� �̵� + ���ڸ� ȸ��
        {
            rotTrans.Rotate(Vector3.up * (90.0f * Time.deltaTime));
            pos = rotTrans.position;
            valueA += Time.deltaTime;
            pos.y = DropPosY +0.3f * Mathf.Sin(valueA);
            rotTrans.position = pos;
        }
    }
    // NPC
    // �浹üũ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
            DropPosY = transform.position.y;
            isDrop = true;
        }

        if(isDrop && other.CompareTag("Player"))
        {
            InventoryItemData newData = new InventoryItemData();
            // ���߿� ���Ϳ� ���� ����Ǵ� �۾��̵� ����
            newData.itemID = 2002;
            newData.amount = 1;

            if (GameManager.Inst.LootingItem(newData))
            {
                Destroy(gameObject);
            }
        }
        
    }
}
