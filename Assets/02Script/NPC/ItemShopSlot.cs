using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 아이템 표기.
// 판매 갯수 결정
// 판매 정보를 ItemShopPopup에 전달.

public class ItemShopSlot : MonoBehaviour
{
    // 슬롯을 소유한 부모 팝업 정보가 필요
    private ItemShopPopup popup; // 직접 참조(결합도가 높다). 간접참조(interface) or 대리자(delegate)

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI tradeCountText;

    [SerializeField] private Button leftBTN;
    [SerializeField] private Button rightBTN;

    private int slotIndex;
    public int SlotIndex => slotIndex;//외부에서만 읽기가 가능하도록 getter로 만들기

    private int totalGold;
    public int TotalGold
    {
        get => totalGold;
        set
        {
            totalGold = value;
            // 델리게이트

            if(OnTotalChange != null)//자신을 구독하는 구독자들에게 변화가 있다고 알림을 줌
                OnTotalChange.Invoke();
        }
    }

    public delegate void TotalGoldChange();
    public event TotalGoldChange OnTotalChange;


    private int tradeGold; // 1개 단가
    private int tradeMaxCount; // 거래 가능한 최대 갯수
    private int tradeCurCount; // 거래하려고 등록한 갯수
    private int itemID;
    private InventoryItemData data; // 어떤 아이템을 몇개를 가지고 있는지

    private void Awake()
    {
        leftBTN.onClick.AddListener(OnClickLeftBTN);
        rightBTN.onClick.AddListener(OnClickRightBTN);
    }


    // itemshopPopup에서 호출
    public void CreateSlot(ItemShopPopup owner, int index)
    {
        popup = owner;
        slotIndex = index;
        icon.enabled = false;
        gameObject.SetActive(false);
    }
    // 슬롯의 표기를 갱신

    public void RefreshSlot(InventoryItemData data)
    {
        gameObject.SetActive(true);
        itemID = data.itemID;
        tradeMaxCount = data.amount;
        tradeCurCount = 0;
        TotalGold = 0;// 델리게이트도 같이 인보크하기 위해서 대문자 사용

        if(DataManager.Inst.GetItemData(itemID, out ItemData_Entity tableInfo))
        { 
            icon.sprite = Resources.Load<Sprite>(tableInfo.iconImg); // << 부담이 큰 작업, DP적용
            icon.enabled = true;
            itemPriceText.text = tableInfo.sellGold.ToString();
            tradeGold = tableInfo.sellGold;
            tradeCountText.text = "0";
            tradeCurCount = 0;
        }
        else
        {
            Debug.Log("아이템숍슬롯에서 RefreshSlot함수의 테이블 정보 참조 실패");
        }
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }

    public bool GetBuyInfo(out int _buyItemID, out int _buyCount, out int _buyGold)
    {
        _buyCount = tradeCurCount;
        _buyItemID = itemID;
        _buyGold = totalGold;
        return true;
    }

    // 거래 될 때 해당 슬롯의 정보를 전달.
    public bool GetSellInfo(out int _sellitemID, out int _sellCount, out int _sellGold)
    {
        _sellitemID = itemID;
        _sellGold = totalGold;
        _sellCount = tradeCurCount;

        return true;
    }
    public void OnClickLeftBTN()
    {
        if(tradeCurCount > 0)
            tradeCurCount--;

        // ui표기 갱신
        tradeCountText.text = tradeCurCount.ToString();
        totalGold = tradeGold * tradeCurCount;
    }

    private void OnClickRightBTN()
    {
        if(tradeCurCount < tradeMaxCount)
            tradeCurCount++;

        // ui표기 갱신
        tradeCountText.text = tradeCurCount.ToString();
        totalGold = tradeGold * tradeCurCount;
    }
}
