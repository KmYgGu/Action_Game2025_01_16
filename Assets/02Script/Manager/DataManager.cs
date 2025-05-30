using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>, IManager
//싱글톤으로 저장하면 씬을 바꾸도 데이터가 유지됨
{
    private bool loadData = false;
    private ActionGame originTable;

    //데이터를 키값과 매칭해서 저장해서 키값을 입력하면 데이터를 빠르게 조회
    private Dictionary<int, ItemData_Entity> dicItemData = new Dictionary<int, ItemData_Entity>();
    private Dictionary<int, Monster_Entity> dicMonsterData = new Dictionary<int, Monster_Entity>();

    private List<TipData_Entity> battleTip = new List<TipData_Entity>();
    private List<TipData_Entity> baseSceneTip = new List<TipData_Entity>();
    private List<TipData_Entity> bossSceneTip = new List<TipData_Entity>();
    private int randValue;

    // 배틀 씬에서 바로 작업 테스트 하기 위해
    protected override void DoAwake()
    {
        base.DoAwake();
        InitManager();
    }


    public void InitManager()
    {
        // 리소스 폴더에 있는 테이블을 불러들이고,
        // 사용하기 편하게 자료구조에 옮겨서
        // 데이터가 필요하다고 요청하면 보내주는 역할
        if(!loadData)
        {
            loadData = true;
            originTable = Resources.Load<ActionGame>("ActionGame");
            //리소스 폴더의 기준으로 해당하는 파일 이름을 넣어주면 동적 로딩을 해줌

            //originTable.ItemData
            for(int i = 0; i < originTable.ItemData.Count; i++)
            {
                dicItemData.Add(originTable.ItemData[i]. id,originTable.ItemData[i]);
            }

            for (int i = 0; i < originTable.MonsterData.Count; i++)
            {
                dicMonsterData.Add(originTable.MonsterData[i].id, originTable.MonsterData[i]);
            }

            for(int i = 0; i < originTable.TipMess.Count; i++)
            {
                if(originTable.TipMess[i].sceneName == SceneName.BaseScene.ToString())
                {
                    baseSceneTip.Add(originTable.TipMess[i]);
                }
                if (originTable.TipMess[i].sceneName == SceneName.BattleScene.ToString())
                {
                    battleTip.Add(originTable.TipMess[i]);
                }
                if (originTable.TipMess[i].sceneName == SceneName.BossScene.ToString())
                {
                    bossSceneTip.Add(originTable.TipMess[i]);
                }

            }
        }

    }

    public string GetTipMessage(SceneName sceneName)
    {
        string result = "";
        switch(sceneName)
        {
            case SceneName.BaseScene:

                randValue = Random.Range(0, baseSceneTip.Count);
                result = baseSceneTip[randValue].tipText;
                break;
            case SceneName.BattleScene:
                randValue = Random.Range(0, battleTip.Count);
                result = battleTip[randValue].tipText;
                break;
            case SceneName.BossScene:
                randValue = Random.Range(0, bossSceneTip.Count);
                result = bossSceneTip[randValue].tipText;
                break;

                default:
                result = "과도한 게임은 일상생활에 지장을 초래할 수 있습니다.";
                    break;
        }

        return result;
    }

    //private void Start()//2025/02/03에 주석처리
    //{
    //    InitManager();
    //    StartManager();
    //}
    public void StartManager()
    {
        if(GetItemData(2002, out ItemData_Entity data))
        {
            Debug.Log(data.name);
        }
    }

    public bool GetItemData(int KeyID, out ItemData_Entity itemData)
    {
        return dicItemData.TryGetValue(KeyID, out itemData);
    }

    //out : 해당 클래스 안에 다양한 변수 타입을 가져올 때 편리
    public bool GetMonsterDate(int KeyID, out Monster_Entity monsterData)
    {
        return dicMonsterData.TryGetValue(KeyID, out monsterData);
    }
}
