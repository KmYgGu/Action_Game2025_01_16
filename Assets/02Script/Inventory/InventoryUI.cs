using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임 시작할 때 슬롯오브젝트를 생성.
// 인벤토리가 열릴 때, 정보를 기반으로 해서 각각의 슬롯을 갱신.

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform contectTrans;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventorySlot slot;

    private int currentCount;
    private int maxCount;

    private List<InventoryItemData> dataList; //유저가 가지고 있는 아이템 목록을 참조(게임 매니저의 pDATA)

    private void Awake()
    {
        InitSlot();

    }

    // 슬롯을 생성하고, 초기화
    private void InitSlot()
    {
        maxCount = 18;
        for(int i = 0; i < maxCount; i++)
        {
            //생성된 오브젝트에서 슬롯을 가져온다
            if (Instantiate(slotPrefab, contectTrans).TryGetComponent<InventorySlot>(out slot))
            {
                slot.SLOTINDEX = i;
                slots.Add(slot);
            }
            else
                Debug.Log("인벤토리UI코드에서 InitSlot함수의 슬롯 생성실패");
        }
    }

    // 현 아이템 정보를 참조해서, 슬롯을 각각 갱신
    public void RefreshInventoryUI()
    {
        dataList = GameManager.Inst.INVEN.GetItemList();
        currentCount = GameManager.Inst.INVEN.CurItemCount;
        maxCount = GameManager.Inst.INVEN.MaxCount;

        for(int i = 0; i < maxCount; i++)
        {
            if(i < currentCount && dataList[i].itemID > -1)
            {
                slots[i].DrawItemSlot(dataList[i]);
            }
            else // 빈칸 슬롯
            {
                slots[i].ClearSlot();
            }
            slots[i].SetSelectSlot(false); // 선택되지 않은 슬롯으로 오픈
        }
    }
}

