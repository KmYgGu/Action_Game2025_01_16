using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SkillBase : MonoBehaviour, IActiveSkill
{
    // 외부에서 public으로 값을 가져올 수 있되, 값을 설정할 땐 상속된 파생 클래스에서만 가능함
    public int ManaCost { get; protected set; }

    public int CoolDown { get; protected set; }

    protected bool isCoolDown = false;
    private string skillName;

    protected ActionGameCharControl owner;

    private void Awake()
    {
        if (!TryGetComponent<ActionGameCharControl>(out owner))
            Debug.Log("SkillBase.cs 시작부분에서 스킬owner 참조 실패");
    }

    public void InitSkillInfo(string newName, int newManaCost)
    {
        this.skillName = newName;
        this.ManaCost = newManaCost;
    }


    public abstract void Active(); // 스킬의 기능을 정의
    //자식들의 클래스에서 정의해주겠다고 미루기
        
    
    protected IEnumerator ColldownRoutine()
    {
        isCoolDown = true;
        yield return YieldInstructionCache.WaitForSeconds(CoolDown);
        isCoolDown = false;
    }

    // 마나가 충분하느냐?
    protected bool TryUseSkill()
    {

        // 얼리 리턴
        if (isCoolDown)// 스킬이 쿨타임중이라면
            return false; 

        if(owner == null) //스킬 소유주가 확인이 안될 때
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
