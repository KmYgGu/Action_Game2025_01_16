using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSkillManager : MonoBehaviour
{
    // ��ų ���,
    List<IActiveSkill>activeSkills = new List<IActiveSkill>();

    IActiveSkill curSkill;


    // ������ ��ų�� Ư�� ��ư���� ��Ī ����
    private void Awake()
    {
        // 1�� ��ų
        curSkill = gameObject.AddComponent<FireBall>(); // �������� �����߱� ������ ����
        activeSkills.Add(curSkill);

        // 2�� ��ų
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


    // Į ���������� ǥ���� ��ư�� ���� ����
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
