using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // NavMesh�� �̿��ϱ� ����

// ���Ͱ� ���� �� �ִ� ���°���
public enum AI_State
{
    idle, // ���ڸ� ���
    Roaming, // �ֺ� ��ȸ
    ReturnHome, // ������ġ�� ����
    Chase, // �÷��̾� ����
    Attack, // �÷��̾� ����
    Die, // ���
}
// ���� ���� ��� FSM


// �ΰ����� ���
public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 spawnedPos; // ���� AI ���۵Ǵ� ��ġ
    private Vector3 moveTargetPos; // �̵� ��ǥ ��ǥ
    private GameObject mainTarget; // �� Ÿ�� ������Ʈ

    private AI_State currentState;
    private bool isInit = false;

    private MonsterBase monster; // ���� �ൿ�� ����

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
        agent.isStopped = false; // �̵� ������ ������Ʈ�� �Ӽ� ����
        spawnedPos = transform.position;

        // �ڷ�ƾ ����
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

    // ������ ��ǥ�� �������� �¿� �ݰ��� ��ȸ,
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


    // �÷��̾� ����, ���� ������ ��ġ���� �ʹ� �ָ� �̵��� ���
    // ������ ��ġ�� ����
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
            if(GetDistanceTarget() < 2.5f) // ���̺� ���� ���� �����Ÿ�
            {
                ChangeAIState(AI_State.Attack);
            }
            else 
            {
                SetMoveTarget(mainTarget.transform.position);
            }
            yield return YieldInstructionCache.WaitForSeconds(0.5f); // �δ�Ǵ� ����̱� ������ 1�ʿ� �� ����

        }

        // Ÿ���� �Ҿ������ ��, (�÷��̾ ����ؼ� ���忡�� ����)
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
        // ���ο� �������� �׺� �޽��� �ִ� �������� ���� �Ŀ�
        // �̵��� ��Ű����
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

    // ���� �����ϰ� �ִ� ����� ���� ���� Ÿ���� �ٲٰ���
    public void SetTarget(GameObject newTarget)
    {
        if(currentState == AI_State.Roaming || currentState == AI_State.idle)
        {
            mainTarget = newTarget;
            ChangeAIState(AI_State.Chase); // ���� �߰��ϸ� ����
        }
    }
}
