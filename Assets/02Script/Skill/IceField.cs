using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceField : SkillBase
{
    public override void Active()
    {
        if (TryUseSkill())
        {
            Debug.Log($"{SkillName()} �����Ǵ� ��ų");
            StartCoroutine(ColldownRoutine());
        }
    }
}
