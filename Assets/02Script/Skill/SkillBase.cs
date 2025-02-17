using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillBase : MonoBehaviour, IActiveSkill
{
    // �ܺο��� public���� ���� ������ �� �ֵ�, ���� ������ �� ��ӵ� �Ļ� Ŭ���������� ������
    public int ManaCost { get; protected set; }

    public int CoolDown { get; protected set; }

    protected bool isCoolDown = false;
    private string skillName;

    protected ActionGameCharControl owner;

    private void Awake()
    {
        if (!TryGetComponent<ActionGameCharControl>(out owner))
            Debug.Log("SkillBase.cs ���ۺκп��� ��ųowner ���� ����");
    }

    public void InitSkillInfo(string newName, int newManaCost)
    {
        this.skillName = newName;
        this.ManaCost = newManaCost;
    }


    public abstract void Active(); // ��ų�� ����� ����
    //�ڽĵ��� Ŭ�������� �������ְڴٰ� �̷��
        
    
    protected IEnumerator ColldownRoutine()
    {
        isCoolDown = true;
        yield return YieldInstructionCache.WaitForSeconds(CoolDown);
        isCoolDown = false;
    }

    // ������ ����ϴ���?
    protected bool TryUseSkill()
    {

        // �� ����
        if (isCoolDown)// ��ų�� ��Ÿ�����̶��
            return false; 

        if(owner == null) //��ų �����ְ� Ȯ���� �ȵ� ��
            return false;

        if(owner.ManaPoint >= ManaCost)
        {
            owner.ManaPoint -= ManaCost;
            return true;
        }

        return false;
    }
    public string SkillName()
    {
        return skillName;
    }
}
