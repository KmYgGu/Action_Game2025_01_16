using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//특정 아이템을 몇개 가지고 있는가?
[System.Serializable]
public class InventoryItemData
{
    public int itemID;  //테이블 데이터의 id
    public int amount;  //몇개를 소유하고 있는가?
    public int uID; //겹치지 않는 아이템의 고유 id

}

[System.Serializable]
public class InventoryData
{
    private int maxItemSlot = 18; //인벤토리의 슬롯 갯수
    public int MaxCount => maxItemSlot;

    private int curItemCount;
    public int CurItemCount
    {
        get => curItemCount;
        set => curItemCount = value;
    }

    private List<InventoryItemData> items = new List<InventoryItemData>();

    // 습득하는 기능
    public void AddItem(InventoryItemData newitem)
    {
        int index = FindItemindex(newitem);

        if(DataManager.Inst.GetItemData(newitem.itemID, out ItemData_Entity newItemData))
        {
            if (newItemData.equip)// 장착 가능아이템일 경우 중복 슬롯이 불가능
            {
                newitem.uID = 1; // todo : UID생성기를 통해 겹치지 않는 UID 생성
                if (!isFull())
                {
                    items.Add(newitem);
                    curItemCount++;
                }
                else
                {
                    Debug.Log("inventoryData.cs - AddItem() - 신규 아이템 추가 실패");
                }
                
            }
            //장착한 불가능한 아이템
            else if (index < 0)// 인벤토리에 똑같은 아이템이 없는 경우
            {
                newitem.uID = -1;
                if (!isFull())
                {
                    items.Add(newitem);
                    curItemCount++;
                }
            }
            else// 인벤토리에 똑같은 아이템이 있는 경우
            {
                items[index].amount += newitem.amount;
            }
        }
        else
        {
            Debug.Log($"InventoryData.cs - AddItem() - {newitem.itemID} 테이블정보 없음");
        }
    }
    // UI에 표기하기 위해서 외부에서 데이터를 참조
    public List<InventoryItemData> GetItemList()
    {
        curItemCount = items.Count;
        return items;
    } 
    // 상점에 아이템을 판매, 소모했을 때 제거
    public int DeleteItem(InventoryItemData deleteitem)
    {
        // 물약을 100가지고 있는 상태에서 상점에 10개를 판매
        int index = FindItemindex(deleteitem);

        if(index < 0) // 찾지 못한 상황
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
                    items.RemoveAt(index); // RemoveAt 인덱스의 원소를 제거.
                    curItemCount--;
                }
            }
        }

        return 0; // 아이템 삭제를 성공
    } 
    // 아이템의 강화를 통해 아이템의 정보가 바뀌어짐
    // data : NPC를 통해서 강화를 시도한 아이템을 정보를 넘겨받음
    public void UpdateItemInfo(InventoryItemData data)
    {
        for( int i = 0; i < items.Count; i++ )
        {
            if (items[i].uID == data.uID)
            {
                items[i].uID += 1;
            }
        }
    }
        
    // 인벤토리 가득차있는지 확인
    public bool isFull()
    {
        return curItemCount >= maxItemSlot;
    }
    // 현 인벤토리에 이미 있는 아이템을 습득 했으면 중첩
    private int FindItemindex(InventoryItemData newitem)
    {
        for(int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].itemID == newitem.itemID)
            {
                return i;
            }
        }
        return -1; // 동일한 타입의 아이템이 존재하지 않는다.
    }

}
