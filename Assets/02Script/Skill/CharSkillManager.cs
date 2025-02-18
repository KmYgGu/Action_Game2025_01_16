using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkillManager : MonoBehaviour
{
    // 스킬 목록,
    List<IActiveSkill>activeSkills = new List<IActiveSkill>();

    IActiveSkill curSkill;


    // 정해진 스킬을 특정 버튼별로 매칭 관리
    private void Awake()
    {
        // 1번 스킬
        curSkill = gameObject.AddComponent<FireBall>(); // 다형성을 구현했기 때문에 가능
        activeSkills.Add(curSkill);

        // 2번 스킬
        curSkill = gameObject.AddComponent<IceField>();
        activeSkills.Add(curSkill);
    }
    private void OnEnable()
    {
        UIManager.OnAttackButtonPressed += PerformAttack;
        UIManager.OnSkill01ButtonPressed += PerformSkill01;
        UIManager.OnSkill02ButtonPressed += PerformSkill02;
        UIManager.OnSkill03ButtonPressed += PerformSkill03;
    }


    // 칼 아이콘으로 표기한 버튼에 대한 반응
    private void PerformAttack()
    {
        activeSkills[0].Active();
    }
    private void PerformSkill01()
    {
        activeSkills[1].Active();
    }
    private void PerformSkill02()
    {
        activeSkills[2].Active();
    }
    private void PerformSkill03()
    {
        activeSkills[3].Active();
    }
}
