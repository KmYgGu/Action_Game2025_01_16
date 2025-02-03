using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //저장소(하드 디스크) 파일을 생성하고 쓰고, 읽기

[System.Serializable]//직렬화된 데이터만 정상적으로 문자열로 바뀜
public class PlayerData
{
    public string nickName;
    public int level;
    public int curEXP;
    public int curHP;
    public int maxHP;
    public int curMP;
    public int maxMP;
    public int gold;
    public int uidCounter; //0부터 시작해서 아이템을 습득할때마다 1씩 증가
    public InventoryData inventoryData;

}

// 게임 구동 전반에 필요한 런타임 데이터를 관리
// 저장 / 불러오기
// 인벤토리를 관리하면서 아이템 관리,
// 플레이어 캐릭터의 정보,

public class GameManager : Singleton<GameManager> , IManager
{
    // C# 포인터가 있냐 없느냐
    // * &

    // 원시형 변수 -> 
    // 참조형 변수 -> string 사용자정의클래스타입 => 포인터와 같은 원리
    private PlayerData pData;
    public PlayerData PData// => pData;
    {
        get => pData;
    }

    // 기존 유저들 (이미 게임 데이터가 존재하는 플레이어)
    // 세이브 파일 생성, 
    // 파일을 생성/ 불러오기


    private void Start()
    {

        //CreateUserData("하하하_이녀석");
        //SaveData();
        //string strData = JsonUtility.ToJson(pData);

        //Debug.Log($"확인 {strData}");

        //string data = File.
        //JsonUtility.FromJson<PlayerData>()

        //Debug.Log($"확인 {pData.nickName}");
    }

    // 신규 유저
    public void CreateUserData(string newNickName)
    {
        pData = new PlayerData();

        pData.nickName = newNickName;
        pData.level = 1;
        pData.curEXP = 0;
        pData.curHP = pData.maxHP = 100;
        pData.curMP = pData.maxMP = 100;
        pData.gold = 10000;
        pData.uidCounter = 0;
        pData.inventoryData = new InventoryData();
    }

    #region _Save&Load

    private string dataPath;
    // 게임의 데이터를 파일로 저장
    public void SaveData()
    {
        string data = JsonUtility.ToJson(pData);
        //플레이어 데이터를 문자열로 만들기

        //암호화

        File.WriteAllText(dataPath, data);
    }

    // 파일로 저장된 데이터를 게임의 객체러 불러오기
    public bool LoadData()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            //복호화
            pData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        
        return false;
    }

    // 게임 데이터를 초기화
    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    // 세이브 파일이 있는지 확인하고, 게임 내 데이터로 로드
    public bool TryGetPlayerData()
    {
        if(File.Exists(dataPath))
            return LoadData(); //데이터 로드 성공
        return false;//데이터 로드 실패
    }
    #endregion

    #region _ItemLooting
    public bool LootingItem(InventoryItemData newitem)
    {
        if (!pData.inventoryData.isFull())
        {
            PData.inventoryData.AddItem(newitem);
            Debug.Log("아이템 습득에 성공했습니다");
            return true;
        }// 중첩 아이템습득 로직은 추후 구현

        return false;
    }

    public void InitManager()
    {
        
    }

    public void StartManager()
    {
        //CreateUserData("하하하_이녀석"); //매번 컴퓨터를 포멧하고 키면 참고할 데이터가 없어서 null이 뜸
        dataPath = Application.persistentDataPath + "/Save";
        LoadData();

        //Debug.Log($"닉네임{pData.nickName}  레벨은{pData.level}");

        InventoryItemData newItemInfo = new InventoryItemData();
        newItemInfo.itemID = 2002;
        newItemInfo.amount = 1;

        LootingItem(newItemInfo);

        SaveData();
    }
    #endregion

    public InventoryData INVEN
    {
        get => pData.inventoryData;
    }

    public int PlayerGold
    {
        get => pData.gold;
        set => pData.gold = value;
    }
    public string PlayerNickName
    {
        get => pData.nickName;
    }

    public int ItemUIDMaker
    {
        get
        {
            return ++pData.uidCounter; //21억 4700을 넘지 않는 이상 겹치지 않음
        }
    }

}
