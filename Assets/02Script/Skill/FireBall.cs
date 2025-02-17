using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : SkillBase
{
    public override void Active()
    {
        if (TryUseSkill())
        {
            // ��ų�� �� �� �ִ� ����
            Debug.Log($"{SkillName()} ��ų �ߵ�");
            StartCoroutine(ColldownRoutine()); // ��Ÿ�� ����
        }
    }
}
