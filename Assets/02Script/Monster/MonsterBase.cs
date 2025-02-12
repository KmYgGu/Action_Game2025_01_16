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

    public void TakeDamage(float damage, GameObject attacker)
    {
        
    }

    
}
