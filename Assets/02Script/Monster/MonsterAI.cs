using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // NavMesh를 이용하기 위해

// 몬스터가 취할 수 있는 상태값들
public enum AI_State
{
    idle, // 제자리 대기
    Roaming, // 주변 배회
    ReturnHome, // 생성위치로 복귀
    Chase, // 플레이어 추적
    Attack, // 플레이어 공격
    Die, // 사망
}
// 유한 상태 기계 FSM


// 인공지능 기능
public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 spawnedPos; // 최초 AI 시작되는 위치
    private Vector3 moveTargetPos; // 이동 목표 좌표
    private GameObject mainTarget; // 주 타겟 오브젝트

    private AI_State currentState;
    private bool isInit = false;

    private MonsterBase monster; // 최종 행동을 수행

    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        TryGetComponent<MonsterBase>(out monster);
    }

    public void StartAI()
    {
        isInit = true;
        currentState = AI_State.Roaming;
        mainTarget = null;
        agent.isStopped = false; // 이동 가능한 에이전트로 속성 변경
        spawnedPos = transform.position;

        // 코루틴 실행
    }

    public void ChangeAIState(AI_State newstate)
    {
        if (isInit)
        {
            StopCoroutine(currentState.ToString());
            currentState = newstate;
            StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator idle()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
    }

    // 스폰된 좌표를 기준으로 좌우 반경을 배회,
    IEnumerator Roaming()
    {
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(Random.Range(4f, 6f));
            moveTargetPos.x = Random.Range(-5, 5);
            moveTargetPos.y = 0f;
            moveTargetPos.z = Random.Range(-5, 5);


            SetMoveTarget(spawnedPos + moveTargetPos);
        }
    }


    // 플레이어 전투, 추적 생성된 위치에서 너무 멀리 이동한 경우
    // 생성된 위치로 복귀
    IEnumerator ReturnHome()
    {
        SetMoveTarget(spawnedPos);
        while (true)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            if(agent.remainingDistance < 2f)
            {
                ChangeAIState(AI_State.Roaming);
            }
        }
    }

    IEnumerator Chase()
    {
        while(mainTarget != null)
        {
            if(GetDistanceTarget() < 2.5f) // 테이블 정보 참조 사정거리
            {
                ChangeAIState(AI_State.Attack);
            }
            else 
            {
                SetMoveTarget(mainTarget.transform.position);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f); // 부담되는 기능이기 때문에 1초에 두 번만

        }

        // 타겟을 잃어버렸을 때, (플레이어가 사망해서 월드에서 제거)
        ChangeAIState(AI_State.ReturnHome);
    }

    IEnumerator Attack()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
    }

    IEnumerator Die()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
    }

    private void SetMoveTarget(Vector3 newPos)
    {
        // 새로운 포지션이 네비 메쉬가 있는 영역인지 검증 후에
        // 이동을 시키도록
        agent.SetDestination(newPos);
    }


    private float GetDistanceTarget()
    {
        if(mainTarget != null)
        {
            return Vector3.Distance(transform.position, mainTarget.transform.position);
        }
        return -1;
    }

    // 현재 공격하고 있는 대상이 없을 때만 타겟을 바꾸게함
    public void SetTarget(GameObject newTarget)
    {
        if(currentState == AI_State.Roaming || currentState == AI_State.idle)
        {
            mainTarget = newTarget;
            ChangeAIState(AI_State.Chase); // 적을 발견하면 추적
        }
    }
}
