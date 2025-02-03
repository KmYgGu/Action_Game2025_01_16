using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //�����(�ϵ� ��ũ) ������ �����ϰ� ����, �б�

[System.Serializable]//����ȭ�� �����͸� ���������� ���ڿ��� �ٲ�
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
    public int uidCounter; //0���� �����ؼ� �������� �����Ҷ����� 1�� ����
    public InventoryData inventoryData;

}

// ���� ���� ���ݿ� �ʿ��� ��Ÿ�� �����͸� ����
// ���� / �ҷ�����
// �κ��丮�� �����ϸ鼭 ������ ����,
// �÷��̾� ĳ������ ����,

public class GameManager : Singleton<GameManager> , IManager
{
    // C# �����Ͱ� �ֳ� ������
    // * &

    // ������ ���� -> 
    // ������ ���� -> string ���������Ŭ����Ÿ�� => �����Ϳ� ���� ����
    private PlayerData pData;
    public PlayerData PData// => pData;
    {
        get => pData;
    }

    // ���� ������ (�̹� ���� �����Ͱ� �����ϴ� �÷��̾�)
    // ���̺� ���� ����, 
    // ������ ����/ �ҷ�����


    private void Start()
    {

        //CreateUserData("������_�̳༮");
        //SaveData();
        //string strData = JsonUtility.ToJson(pData);

        //Debug.Log($"Ȯ�� {strData}");

        //string data = File.
        //JsonUtility.FromJson<PlayerData>()

        //Debug.Log($"Ȯ�� {pData.nickName}");
    }

    // �ű� ����
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
    // ������ �����͸� ���Ϸ� ����
    public void SaveData()
    {
        string data = JsonUtility.ToJson(pData);
        //�÷��̾� �����͸� ���ڿ��� �����

        //��ȣȭ

        File.WriteAllText(dataPath, data);
    }

    // ���Ϸ� ����� �����͸� ������ ��ü�� �ҷ�����
    public bool LoadData()
    {
        if (File.Exists(dataPath))
        {
            string data = File.ReadAllText(dataPath);
            //��ȣȭ
            pData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        
        return false;
    }

    // ���� �����͸� �ʱ�ȭ
    public void DeleteData()
    {
        File.Delete(dataPath);
    }

    // ���̺� ������ �ִ��� Ȯ���ϰ�, ���� �� �����ͷ� �ε�
    public bool TryGetPlayerData()
    {
        if(File.Exists(dataPath))
            return LoadData(); //������ �ε� ����
        return false;//������ �ε� ����
    }
    #endregion

    #region _ItemLooting
    public bool LootingItem(InventoryItemData newitem)
    {
        if (!pData.inventoryData.isFull())
        {
            PData.inventoryData.AddItem(newitem);
            Debug.Log("������ ���濡 �����߽��ϴ�");
            return true;
        }// ��ø �����۽��� ������ ���� ����

        return false;
    }

    public void InitManager()
    {
        
    }

    public void StartManager()
    {
        //CreateUserData("������_�̳༮"); //�Ź� ��ǻ�͸� �����ϰ� Ű�� ������ �����Ͱ� ��� null�� ��
        dataPath = Application.persistentDataPath + "/Save";
        LoadData();

        //Debug.Log($"�г���{pData.nickName}  ������{pData.level}");

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
            return ++pData.uidCounter; //21�� 4700�� ���� �ʴ� �̻� ��ġ�� ����
        }
    }

}
