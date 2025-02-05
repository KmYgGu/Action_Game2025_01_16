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

                sellSlotList.Add(itemShopSlot);
            }
        }

        for(int i = 0; i < 4; i++)// 모든 물약종류가 4종류라서
        {
            if(Instantiate(slotPrefab, buyViewContent).TryGetComponent<ItemShopSlot>(out itemShopSlot))
            {
                itemShopSlot.gameObject.name = $"BuySlotM{i}";
                // todo : 슬롯의 데이터 변경을 받아오는 델리게이트 체인 구독, 슬록스크립트 작성 후에

                buySlotList.Add(itemShopSlot);
            }
        }

        sellTapBtn.onClick.AddListener(OnClick_Selltap);
        buyTapBtn.onClick.AddListener(OnClick_Buytap);
        applyBtn.onClick.AddListener(OnClick_Apply);
    }

    public void OnClick_Selltap()
    {
        //sellView 갱신
        sellView.SetActive(true);
        buyView.SetActive(false);
    }
    public void OnClick_Buytap()
    {
        //buyView 갱신
        buyView.SetActive(true);
        sellView.SetActive(false);
        
    }
    public void OnClick_Apply()
    {

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
