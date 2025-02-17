using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : SkillBase
{
    public override void Active()
    {
        if (TryUseSkill())
        {
            // 스킬을 쓸 수 있는 상태
            Debug.Log($"{SkillName()} 스킬 발동");
            StartCoroutine(ColldownRoutine()); // 쿨타임 동작
        }
    }
}
