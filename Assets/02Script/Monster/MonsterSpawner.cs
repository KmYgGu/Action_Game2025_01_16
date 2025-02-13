using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public enum SpawnType
{
    Once,   // 한번에 전부다 스폰
    repeat, // 지속 시간 동안 반복적으로 스폰할 것 인지?
}

[RequireComponent(typeof(SphereCollider))]
public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private int maxCount;
    private int curCount = 0;
    [SerializeField] private SpawnType spawnType;

    [SerializeField] private int spawnMonsterTableID;
    private PoolManager poolManager;

    public PoolManager PoolMANAGER
    {
        get
        {
            // 다이나믹 프로그래밍 : 값을 가져올 때마다 값이 비어있으면 새로 참조
            if(poolManager == null)
            {
                TryGetComponent<PoolManager>(out poolManager);
            }
            return poolManager;
        }
    }

    private void Awake()
    {
        if(TryGetComponent<SphereCollider>(out SphereCollider col))
        {
            col.radius = 20.0f;
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine("TrySpawn");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine("TrySpawn");
        }
    }

    IEnumerator TrySpawn()
    {
        while(true)
        {
            yield return YieldInstructionCache.WaitForSeconds(2.5f);

            if(curCount < maxCount)
            {
                // 몬스터를 스폰
            }
        }
    }

    private Vector3 spawnPos;
    private MonsterBase monster;


    private void SpwanUnit()
    {
        monster = PoolMANAGER.GetFromPool<MonsterBase>(0);

        if(monster != null)
        {
            spawnPos = transform.position;
            spawnPos.x += Random.Range(-10f, 10.0f);
            spawnPos.y += Random.Range(-10f, 10.0f);


            monster.transform.position = spawnPos;
            monster.InitMonster(spawnMonsterTableID, this);
            curCount++;
        }
    }

    public void ReturnPool(MonsterBase returnMonster)
    {
        PoolMANAGER.TakeToPool<MonsterBase>(returnMonster.PoolName, returnMonster);
        curCount--;
    }
}
