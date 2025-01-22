using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Ư�� �������� � ������ �ִ°�?
[System.Serializable]
public class InventoryItemData
{
    public int itemID;  //���̺� �������� id
    public int amount;  //��� �����ϰ� �ִ°�?
    public int uID; //��ġ�� �ʴ� �������� ���� id

}

[System.Serializable]
public class InventoryData
{
    private int maxItemSlot = 18; //�κ��丮�� ���� ����
    public int MaxCount => maxItemSlot;

    private int curItemCount;
    public int CurItemCount
    {
        get => curItemCount;
        set => curItemCount = value;
    }

    private List<InventoryItemData> items = new List<InventoryItemData>();

    // �����ϴ� ���
    public void AddItem(InventoryItemData newitem)
    {
        int index = FindItemindex(newitem);

        if(DataManager.Inst.GetItemData(newitem.itemID, out ItemData_Entity newItemData))
        {
            if (newItemData.equip)// ���� ���ɾ������� ��� �ߺ� ������ �Ұ���
            {
                newitem.uID = 1; // todo : UID�����⸦ ���� ��ġ�� �ʴ� UID ����
                if (!isFull())
                {
                    items.Add(newitem);
                    curItemCount++;
                }
                else
                {
                    Debug.Log("inventoryData.cs - AddItem() - �ű� ������ �߰� ����");
                }
                
            }
            //������ �Ұ����� ������
            else if (index < 0)// �κ��丮�� �Ȱ��� �������� ���� ���
            {
                newitem.uID = -1;
                if (!isFull())
                {
                    items.Add(newitem);
                    curItemCount++;
                }
            }
            else// �κ��丮�� �Ȱ��� �������� �ִ� ���
            {
                items[index].amount += newitem.amount;
            }
        }
        else
        {
            Debug.Log($"InventoryData.cs - AddItem() - {newitem.itemID} ���̺����� ����");
        }
    }
    // UI�� ǥ���ϱ� ���ؼ� �ܺο��� �����͸� ����
    public List<InventoryItemData> GetItemList()
    {
        curItemCount = items.Count;
        return items;
    } 
    // ������ �������� �Ǹ�, �Ҹ����� �� ����
    public int DeleteItem(InventoryItemData deleteitem)
    {
        // ������ 100������ �ִ� ���¿��� ������ 10���� �Ǹ�
        int index = FindItemindex(deleteitem);

        if(index < 0) // ã�� ���� ��Ȳ
        {

            return 10001;
        }
        else
        {
            if (items[index].amount < deleteitem.amount)
            {
                return 10002;
            }
            else
            {
                items[index].amount -= deleteitem.amount;
                if(items[index].amount <= 0)
                {
                    items.RemoveAt(index); // RemoveAt �ε����� ���Ҹ� ����.
                    curItemCount--;
                }
            }
        }

        return 0; // ������ ������ ����
    } 
    // �������� ��ȭ�� ���� �������� ������ �ٲ����
    public void UpdateItemInfo(InventoryItemData item)
    {

    }

    // �κ��丮 �������ִ��� Ȯ��
    public bool isFull()
    {
        return false;
    }
    // �� �κ��丮�� �̹� �ִ� �������� ���� ������ ��ø
    private int FindItemindex(InventoryItemData item)
    {
        return 0;
    }

}
