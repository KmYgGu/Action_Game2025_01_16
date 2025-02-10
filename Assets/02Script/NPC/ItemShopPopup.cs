using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShopPopup : MonoBehaviour, IPopUp
{

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] RectTransform buyViewContent; // ������ �������� �÷��̾ �����ϴ� ��
    [SerializeField] RectTransform sellViewContent; // �÷��̾��� �������� ���ο��� �ѱ�� ��
    [SerializeField] TextMeshProUGUI balanceText; // �÷��̾� ��带 ǥ��
    [SerializeField] TextMeshProUGUI tradeText; // �ŷ� ��ǰ�� ���� �Ѿ�
    [SerializeField] Button sellTapBtn;
    [SerializeField] Button buyTapBtn;
    [SerializeField] Button applyBtn;
    [SerializeField] GameObject buyView;
    [SerializeField] GameObject sellView;

    //������Ʈ Ǯ��ó�� ������ �̸� �����
    List<ItemShopSlot> buySlotList = new List<ItemShopSlot>();
    List<ItemShopSlot> sellSlotList = new List<ItemShopSlot>();

    List<InventoryItemData> userInven; // ���� �Ŵ����� �����ϰ� �ִ� pData �κ��丮 ����Ʈ�� ����

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
                // todo : ������ ������ ������ �޾ƿ��� ��������Ʈ ü�� ����, ���Ͻ�ũ��Ʈ �ۼ� �Ŀ�

                itemShopSlot.CreateSlot(this, i);
                itemShopSlot.OnTotalChange += CalculateGold;

                sellSlotList.Add(itemShopSlot);
            }
        }

        for(int i = 0; i < 4; i++)// ��� ���������� 4������
        {
            if(Instantiate(slotPrefab, buyViewContent).TryGetComponent<ItemShopSlot>(out itemShopSlot))
            {
                itemShopSlot.gameObject.name = $"BuySlotM{i}";
                // todo : ������ ������ ������ �޾ƿ��� ��������Ʈ ü�� ����, ���Ͻ�ũ��Ʈ �ۼ� �Ŀ�

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
        //sellView ����
        sellView.SetActive(true);
        buyView.SetActive(false);
    }
    public void OnClick_Buytap()
    {
        RefreshBuyViewData();
        //buyView ����
        buyView.SetActive(true);
        sellView.SetActive(false);
        
    }
    private int itemID, tradeCount, tradeGold, totalGold;

    // �ŷ� ���簡 �Ǿ��� ��.
    // �÷��̾� �����Ϳ��� ��带 �Ҹ��ϰų�. �������� ����. ������ް� ������ �Ҹ�
    public void OnClick_Apply()
    {
        if(sellView.activeSelf)// �Ǹ� ���� �������� ��
        {
            // ������ for��
            for(int i = inventory.CurItemCount -1; 1 >= 0; i--) // ������ ��Ͽ��� ����
            {
                // todo �Ǹ��� �������� ������ itemslot���κ��� �޾ƿ´�
                sellSlotList[i].GetSellInfo(out itemID, out tradeCount, out tradeGold);
                // ��带 ����ó��
                GameManager.Inst.PlayerGold += tradeGold;

                // �����͸� �����ϴ� inventroy���� ���� �������� ����

                InventoryItemData itemData = new InventoryItemData();
                itemData.itemID = itemID;
                itemData.amount = tradeCount;

                inventory.DeleteItem(itemData);     
            }

            OnClick_Selltap(); // << sell Tap �����۾�

        }
        else// ������
        {
            totalGold = 0;

            for(int i =0; i < 4; i++)
            {
                // ��� �������� �ŷ����� ���� �޾ƿ���,
                buySlotList[i].GetBuyInfo(out itemID, out tradeCount,out tradeGold);
                totalGold += tradeGold;
            }

            if(totalGold <= GameManager.Inst.PlayerGold)// ������ �� �ִ� ����
            {
                GameManager.Inst.PlayerGold -= totalGold;

                for(int i = 0; i < 4; i++)
                {
                    // ������ ���� �޾ƿ���
                    buySlotList[i].GetBuyInfo(out itemID, out tradeCount, out tradeGold);
                    if(tradeCount > 0)
                    {
                        InventoryItemData itemData = new InventoryItemData();
                        itemData.itemID = itemID;
                        itemData.amount = tradeCount;
                        inventory.AddItem(itemData); // ������ ����
                    }
                }
            }
            OnClick_Buytap(); // ���� �� ����
        }
    }

    // �ŷ� �Ѿװ� �÷��̾� ���� ��尡 ��ȭ ���� �� UI ����
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

    // �Ǹ� ����� UI�� ����
    private void RefreshSellViewData()
    {
        userInven = GameManager.Inst.INVEN.GetItemList();// ���� ����

        for(int i = 0;i < inventory.MaxCount; i++)
        {
            // ���������� �����ϰ� �ִ� ���������� �˻�
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
                // buyslot ����
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
        OnClick_Buytap();// buyTapȭ�� ����
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}


