using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShopPopup : MonoBehaviour, IPopUp
{

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] RectTransform buyViewContent; // 상인의 아이템을 플레이어가 구매하는 것
    [SerializeField] RectTransform sellViewContent; // 플레이어의 아이템을 상인에게 넘기는 것
    [SerializeField] TextMeshProUGUI balanceText; // 플레이어 골드를 표기
    [SerializeField] TextMeshProUGUI tradeText; // 거래 물품에 대한 총액
    [SerializeField] Button sellTapBtn;
    [SerializeField] Button buyTapBtn;
    [SerializeField] Button applyBtn;
    [SerializeField] GameObject buyView;
    [SerializeField] GameObject sellView;

    //오브젝트 풀링처럼 슬롯을 미리 만들기
    List<ItemShopSlot> buySlotList = new List<ItemShopSlot>();
    List<ItemShopSlot> sellSlotList = new List<ItemShopSlot>();

    List<InventoryItemData> userInven; // 게임 매니저가 관리하고 있는 pData 인벤토리 리스트를 참조

    private InventoryData inventory;
    private ItemShopSlot itemShopSlot;

    private void Start()
    {
        InitPopup();
        gameObject.LeanScale(Vector3.zero, 0.1f);
    }

    private void InitPopup()
    {
        inventory = GameManager.Inst.INVEN;
        for(int i = 0; i< inventory.MaxCount; i++)
        {
            if(Instantiate(slotPrefab, sellViewContent).TryGetComponent<ItemShopSlot>(out itemShopSlot))
                {
                itemShopSlot.gameObject.name = $"SellSlot_{i}";
                // todo : 슬롯의 데이터 변경을 받아오는 델리게이트 체인 구독, 슬록스크립트 작성 후에

                itemShopSlot.CreateSlot(this, i);
                itemShopSlot.OnTotalChange += CalculateGold;

                sellSlotList.Add(itemShopSlot);
            }
        }

        for(int i = 0; i < 4; i++)// 모든 물약종류가 4종류라서
        {
            if(Instantiate(slotPrefab, buyViewContent).TryGetComponent<ItemShopSlot>(out itemShopSlot))
            {
                itemShopSlot.gameObject.name = $"BuySlotM{i}";
                // todo : 슬롯의 데이터 변경을 받아오는 델리게이트 체인 구독, 슬록스크립트 작성 후에

                itemShopSlot.CreateSlot(this, i);
                itemShopSlot.OnTotalChange += CalculateGold;

                buySlotList.Add(itemShopSlot);
            }
        }

        sellTapBtn.onClick.AddListener(OnClick_Selltap);
        buyTapBtn.onClick.AddListener(OnClick_Buytap);
        applyBtn.onClick.AddListener(OnClick_Apply);
    }

    public void OnClick_Selltap()
    {
        RefreshSellViewData();
        //sellView 갱신
        sellView.SetActive(true);
        buyView.SetActive(false);
    }
    public void OnClick_Buytap()
    {
        RefreshBuyViewData();
        //buyView 갱신
        buyView.SetActive(true);
        sellView.SetActive(false);
        
    }
    private int itemID, tradeCount, tradeGold, totalGold;

    // 거래 성사가 되었을 때.
    // 플레이어 데이터에서 골드를 소모하거나. 아이템을 지급. 골드지급과 아이템 소모
    public void OnClick_Apply()
    {
        if(sellView.activeSelf)// 판매 탭이 열려있을 때
        {
            // 리버스 for문
            for(int i = inventory.CurItemCount -1; 1 >= 0; i--) // 아이템 목록에서 삭제
            {
                // todo 판매할 아이템의 갯수를 itemslot으로부터 받아온다
                sellSlotList[i].GetSellInfo(out itemID, out tradeCount, out tradeGold);
                // 골드를 증가처리
                GameManager.Inst.PlayerGold += tradeGold;

                // 데이터를 관리하는 inventroy에서 실제 아이템을 삭제

                InventoryItemData itemData = new InventoryItemData();
                itemData.itemID = itemID;
                itemData.amount = tradeCount;

                inventory.DeleteItem(itemData);     
            }

            OnClick_Selltap(); // << sell Tap 갱신작업

        }
        else// 구매탭
        {
            totalGold = 0;

            for(int i =0; i < 4; i++)
            {
                // 몇개의 아이템이 거래될지 정보 받아오고,
                buySlotList[i].GetBuyInfo(out itemID, out tradeCount,out tradeGold);
                totalGold += tradeGold;
            }

            if(totalGold <= GameManager.Inst.PlayerGold)// 구매할 수 있는 상태
            {
                GameManager.Inst.PlayerGold -= totalGold;

                for(int i = 0; i < 4; i++)
                {
                    // 슬롯의 정보 받아오기
                    buySlotList[i].GetBuyInfo(out itemID, out tradeCount, out tradeGold);
                    if(tradeCount > 0)
                    {
                        InventoryItemData itemData = new InventoryItemData();
                        itemData.itemID = itemID;
                        itemData.amount = tradeCount;
                        inventory.AddItem(itemData); // 아이템 지급
                    }
                }
            }
            OnClick_Buytap(); // 구매 탭 갱신
        }
    }

    // 거래 총액과 플레이어 보유 골드가 변화 했을 때 UI 갱신
    public void RefreshGold()
    {
        tradeText.text = totalGold.ToString();
        balanceText.text = GameManager.Inst.PlayerGold.ToString();
    }

    public void CalculateGold()
    {
        totalGold = 0;
        //totalGold = 999;

        if (sellView.activeSelf)
        {
            for (int i = 0; i < sellSlotList.Count; i++)
            {
                if (sellSlotList[i].isActiveAndEnabled)
                {
                    totalGold += sellSlotList[i].TotalGold;
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (buySlotList[i].isActiveAndEnabled)
                {
                    totalGold += sellSlotList[i].TotalGold;
                }
            }
            RefreshGold();
        }
        
    }

    // 판매 목록을 UI로 갱신
    private void RefreshSellViewData()
    {
        userInven = GameManager.Inst.INVEN.GetItemList();// 얕은 복사

        for(int i = 0;i < inventory.MaxCount; i++)
        {
            // 정상적으로 소유하고 있는 아이템인지 검사
            if (i < inventory.CurItemCount && -1 < userInven[i].itemID)
            {
                sellSlotList[i].RefreshSlot(userInven[i]);
            }
            else
            {
                    sellSlotList[i].ClearSlot();
            }
        }
        totalGold = 0;
        RefreshGold();
    }

    private void RefreshBuyViewData()
    {
        InventoryItemData itemData = new InventoryItemData();
        for(int i =0; i < 4; i++)
        {
            itemData.itemID = 2001001 + i;
            itemData.amount = 999;
                // buyslot 갱신
                buySlotList[i].RefreshSlot(itemData);
        }
        totalGold = 0;
        RefreshGold();
    }

    public void PopupClose()
    {
        gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }

    public void PopupOpen()
    {
        OnClick_Buytap();// buyTap화면 갱신
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}


