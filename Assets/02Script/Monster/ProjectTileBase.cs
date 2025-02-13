using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools; // 레드코드 회사에서 만들어 판매하는 에셋

public class ProjectTileBase : MonoBehaviour, IPoolObject
{
    [SerializeField] private float lifeTime = 10; // 생성 이후에 해당 시간만큼 월드에 존재하다가. 그 뒤에는 자연 소멸
    private float selfDestoryTime; // 소멸 예정 시간을 체크

    [SerializeField] private string poolManagerName;
    private PoolManager poolManager; // 레드 코드에서 제공하는 풀 매니저 클래스
    private string ownerTag; // 프로젝타일을 생성한 부모의 tag

    private bool isInit; // 정보세팅완료
    private ParticleSystem particle; // 터지는 타이밍에 재생하기 위해 파티클 참조
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
        selfDestoryTime = Time.time + lifeTime; // 소멸되는 시간은 현재의 시간에 미래의 시간에 더해, 이 시간이 과거의 시간이 되면 소멸
        attackDamage = newDamage;
        ownerTag = newTag;
        poolManager = pool;



        hamerObj?.SetActive(true);// 해머가 null인지 검사하고 null이 아닐 경우에 뒤의 함수를 실행
        isInit = true;
    }


    private void Update()
    {
        if (isInit)// 초기화가 정상적으로 처리 되었을 때
        {
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
            transform.Rotate(-Vector3.forward * (720f * Time.deltaTime));
            if(Time.time > selfDestoryTime)// 이 오브젝트가 소멸되어야 하는 시간
            {
                Explosion();
            }
        }
    }

    // 오너를 제외한 다른 오브젝트와 충돌했을 때, 터지도록
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(ownerTag))
        {
            Explosion();
        }
    }

    private void Explosion()// 주변에 데미지를 주기 전에
    {
        ApplyDamage(); // 주변에 데미지를 주고
        particle.Play();
        isInit = false;
        hamerObj.SetActive(false);

        Invoke("ReturnPool", 3f);
    }

    private void ApplyDamage()
    {
       Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

        for(int i = 0; i < colliders.Length; i++)// 몇몇 자료구조는 foreach를 사용하면 성능에 지장을 줌
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
        // 해당 오브젝트를 매니저에 미리 생성해줄 때, 한 번 호출되는 이벤트
    }

    public void OnGettingFromPool()
    {
       // 해당 오브젝트가 매니저에서 쓰기 위해서 활성화 해올 때, 호출되는 이벤트
    }
}
