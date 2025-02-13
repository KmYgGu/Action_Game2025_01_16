using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 몬스터 애니메이션,
// 몬스터 체력관리
// 몬스터 피격 체크
public class MonsterBase : MonoBehaviour, IDamaged
{
    private NavMeshAgent agent;
    private Animator anims;

    private int animHash_Walk = Animator.StringToHash("isWalk");
    private int animHash_Die = Animator.StringToHash("isDead");
    private int animHash_Attack = Animator.StringToHash("DoAttack");
    private int animHash_Hit = Animator.StringToHash("GetHit");


    private GameObject obj;

    private Monster_Entity curData;
    private MonsterAI monsterAI;
    private MonsterSpawner owner;
    private float currentHP;

    // 자신을 관리해주는 풀의 이름
    [SerializeField] private string poolName;
    public string PoolName => poolName;//읽기 전용





    private void Awake()
    {
        obj = GameObject.Find("MainPlayer");

        TryGetComponent<NavMeshAgent>(out agent);
        if (obj != null)
        {
            // 해당 에이전트의 이동 목적지 설정
            agent.SetDestination(obj.transform.position);
        }

        transform.GetChild(0).TryGetComponent<Animator>(out anims);
    }

    private void Update()
    {
        if(agent != null)
        {
            if(agent.velocity.sqrMagnitude > 0.2f)
            {
                anims.SetBool(animHash_Walk, true);
            }
            else
            {
                anims.SetBool(animHash_Walk, false);
            }
            
        }
    }

    virtual public void InitMonster(int tableID, MonsterSpawner ownerSpawner)
    {
        DataManager.Inst.GetMonsterDate(tableID, out curData);
        owner = ownerSpawner;

        currentHP = curData.maxHp;
        agent.speed = curData.moveSpeed;
        // 몬스터가 타겟 위치의 어느 정도 거리에 접근하면 도착했다고 인식
        agent.stoppingDistance = 2f;

        // 몬스터가 죽었을때랑 살았을 때를 구별하기 위한 레이어 설정
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        if(TryGetComponent<MonsterAI>(out monsterAI))
        {
            monsterAI.StartAI();
        }
    }

    public float CalculateDamage(float takeDamage)
    {
        float resultDamage = takeDamage;

        // 데미지 공식들 적용

        resultDamage = 1; // todo : 데미지 공식 만들고 나서 수정
        return resultDamage;
    }


    public void AttackTarget()
    {
        // AI에 의해 호출되는 공격 처리.
        anims.SetTrigger(animHash_Attack);
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        if(currentHP > 0)
        {
            currentHP -= CalculateDamage(damage);
            if(currentHP <= 0)
            {// 사망처리
                StartCoroutine(OnDie());
            }
            else
            {// 데미지 상황
                StartCoroutine(OnHit());
            }
        }
    }

    IEnumerator OnDie()
    {
        anims.SetTrigger(animHash_Die);
        monsterAI.ChangeAIState(AI_State.Die);
        gameObject.layer = LayerMask.NameToLayer("DieChar");

        yield return YieldInstructionCache.WaitForSeconds(2.5f);

        
        owner.ReturnPool(this);
    }

    IEnumerator OnHit()
    {
        anims.SetTrigger(animHash_Hit);
        yield return null;

    }

    
}
