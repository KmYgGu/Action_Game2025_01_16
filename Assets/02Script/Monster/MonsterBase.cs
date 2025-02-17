using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ���� �ִϸ��̼�,
// ���� ü�°���
// ���� �ǰ� üũ
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

    // �ڽ��� �������ִ� Ǯ�� �̸�
    [SerializeField] private string poolName;
    public string PoolName => poolName;//�б� ����


    // ���Ͱ� �߻���Ű�� ������Ÿ���� �����ϴ� Ǯ �Ŵ���
    private ProjectTileBase projectile;
    [SerializeField] Transform attackTrans; // << ������Ÿ�� ���� ��ġ
    private PoolManager projectileManager;
    public PoolManager poolMGR
    {
        get
        {
            if(projectileManager == null)
            {
                TryGetComponent<PoolManager>(out projectileManager);
               
            }
            return projectileManager;
        }
    }





    private void Awake()
    {
        obj = GameObject.Find("MainPlayer");

        TryGetComponent<NavMeshAgent>(out agent);
        if (obj != null)
        {
            // �ش� ������Ʈ�� �̵� ������ ����
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
        // ���Ͱ� Ÿ�� ��ġ�� ��� ���� �Ÿ��� �����ϸ� �����ߴٰ� �ν�
        agent.stoppingDistance = 2f;

        // ���Ͱ� �׾������� ����� ���� �����ϱ� ���� ���̾� ����
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        if(TryGetComponent<MonsterAI>(out monsterAI))
        {
            monsterAI.StartAI();
        }
    }

    public float CalculateDamage(float takeDamage)
    {
        float resultDamage = takeDamage;

        // ������ ���ĵ� ����

        resultDamage = 1; // todo : ������ ���� ����� ���� ����
        return resultDamage;
    }


    public void AttackTarget()
    {
        // AI�� ���� ȣ��Ǵ� ���� ó��.
        anims.SetTrigger(animHash_Attack);
    }

    public void SpawnProjectile()
    {
        Debug.Log("ȣ��");
        // ���⿡�� ����
        projectile = poolMGR.GetFromPool<ProjectTileBase>(0);
        projectile.transform.position = attackTrans.position;
        projectile.transform.LookAt(attackTrans.position + transform.forward);
        // ������ ��ġ + ������ ����

        projectile.InitProjectile(transform.forward, 10.0f, 10.0f, curData.attackDamage,
            transform.tag, poolMGR);

    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        if(currentHP > 0)
        {
            currentHP -= CalculateDamage(damage);
            if(currentHP <= 0)
            {// ���ó��
                StartCoroutine(OnDie());
            }
            else
            {// ������ ��Ȳ
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
