using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceField : SkillBase
{
    public override void Active()
    {
        if (TryUseSkill())
        {
            Debug.Log($"{SkillName()} 유지되는 스킬");
            StartCoroutine(ColldownRoutine());
        }
    }
}
