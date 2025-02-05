using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>, IManager
//�̱������� �����ϸ� ���� �ٲٵ� �����Ͱ� ������
{
    private bool loadData = false;
    private ActionGame originTable;

    //�����͸� Ű���� ��Ī�ؼ� �����ؼ� Ű���� �Է��ϸ� �����͸� ������ ��ȸ
    private Dictionary<int, ItemData_Entity> dicItemData = new Dictionary<int, ItemData_Entity>();
    private Dictionary<int, Monster_Entity> dicMonsterData = new Dictionary<int, Monster_Entity>();

    private List<TipData_Entity> battleTip = new List<TipData_Entity>();
    private List<TipData_Entity> baseSceneTip = new List<TipData_Entity>();
    private List<TipData_Entity> bossSceneTip = new List<TipData_Entity>();
    private int randValue;

    public void InitManager()
    {
        // ���ҽ� ������ �ִ� ���̺��� �ҷ����̰�,
        // ����ϱ� ���ϰ� �ڷᱸ���� �Űܼ�
        // �����Ͱ� �ʿ��ϴٰ� ��û�ϸ� �����ִ� ����
        if(!loadData)
        {
            originTable = Resources.Load<ActionGame>("ActionGame");
            //���ҽ� ������ �������� �ش��ϴ� ���� �̸��� �־��ָ� ���� �ε��� ����

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
                result = "������ ������ �ϻ��Ȱ�� ������ �ʷ��� �� �ֽ��ϴ�.";
                    break;
        }

        return result;
    }

    //private void Start()//2025/02/03�� �ּ�ó��
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

    //out : �ش� Ŭ���� �ȿ� �پ��� ���� Ÿ���� ������ �� ��
    public bool GetMonsterDate(int KeyID, out Monster_Entity monsterData)
    {
        return dicMonsterData.TryGetValue(KeyID, out monsterData);
    }
}
