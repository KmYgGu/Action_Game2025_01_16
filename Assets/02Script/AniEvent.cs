using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniEvent : MonoBehaviour
{
    public void AnimEvent_SpownProjTile()
    {
        //Debug.Log("�ִϸ��̼� ����� ���� �̺�Ʈ �߻�");

        // ���յ��� ���缭 ��Ŀ�ø��� �����Ѵ�
        transform.parent.SendMessage("SpawnProjectile");

        // �ظ��ϸ� ��������Ʈ�� �ذ�
    }
}
