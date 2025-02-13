using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools; // �����ڵ� ȸ�翡�� ����� �Ǹ��ϴ� ����

public class ProjectTileBase : MonoBehaviour, IPoolObject
{
    [SerializeField] private float lifeTime = 10; // ���� ���Ŀ� �ش� �ð���ŭ ���忡 �����ϴٰ�. �� �ڿ��� �ڿ� �Ҹ�
    private float selfDestoryTime; // �Ҹ� ���� �ð��� üũ

    [SerializeField] private string poolManagerName;
    private PoolManager poolManager; // ���� �ڵ忡�� �����ϴ� Ǯ �Ŵ��� Ŭ����
    private string ownerTag; // ������Ÿ���� ������ �θ��� tag

    private bool isInit; // �������ÿϷ�
    private ParticleSystem particle; // ������ Ÿ�ֿ̹� ����ϱ� ���� ��ƼŬ ����
    private GameObject hamerObj;
    private Vector3 moveDir;
    private float moveSpeed;
    private float attackDamage;


    private void Awake()
    {
        hamerObj = transform.GetChild(0).gameObject;
        hamerObj?.SetActive(false);
        transform.GetChild(1).TryGetComponent<ParticleSystem>(out particle);
    }

    public void InitProjectile(Vector3 newDir, float newSpeed, float newLifeTime, float newDamage, string newTag, PoolManager pool)
    {
        moveDir = newDir;
        moveSpeed = newSpeed;
        lifeTime = newLifeTime;
        selfDestoryTime = Time.time + lifeTime; // �Ҹ�Ǵ� �ð��� ������ �ð��� �̷��� �ð��� ����, �� �ð��� ������ �ð��� �Ǹ� �Ҹ�
        attackDamage = newDamage;
        ownerTag = newTag;
        poolManager = pool;



        hamerObj?.SetActive(true);// �ظӰ� null���� �˻��ϰ� null�� �ƴ� ��쿡 ���� �Լ��� ����
        isInit = true;
    }


    private void Update()
    {
        if (isInit)// �ʱ�ȭ�� ���������� ó�� �Ǿ��� ��
        {
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
            transform.Rotate(-Vector3.forward * (720f * Time.deltaTime));
            if(Time.time > selfDestoryTime)// �� ������Ʈ�� �Ҹ�Ǿ�� �ϴ� �ð�
            {
                Explosion();
            }
        }
    }

    // ���ʸ� ������ �ٸ� ������Ʈ�� �浹���� ��, ��������
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(ownerTag))
        {
            Explosion();
        }
    }

    private void Explosion()// �ֺ��� �������� �ֱ� ����
    {
        ApplyDamage(); // �ֺ��� �������� �ְ�
        particle.Play();
        isInit = false;
        hamerObj.SetActive(false);

        Invoke("ReturnPool", 3f);
    }

    private void ApplyDamage()
    {
       Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

        for(int i = 0; i < colliders.Length; i++)// ��� �ڷᱸ���� foreach�� ����ϸ� ���ɿ� ������ ��
        {
            if(!colliders[i].CompareTag(ownerTag) && colliders[i].TryGetComponent<IDamaged>(out IDamaged damage))
            {
                damage.TakeDamage(attackDamage, gameObject);
            }
        }
    }

    public void ReturnPool()
    {
        poolManager.TakeToPool<ProjectTileBase>(poolManagerName, this);
    }
    public void OnCreatedInPool()
    {
        // �ش� ������Ʈ�� �Ŵ����� �̸� �������� ��, �� �� ȣ��Ǵ� �̺�Ʈ
    }

    public void OnGettingFromPool()
    {
       // �ش� ������Ʈ�� �Ŵ������� ���� ���ؼ� Ȱ��ȭ �ؿ� ��, ȣ��Ǵ� �̺�Ʈ
    }
}
