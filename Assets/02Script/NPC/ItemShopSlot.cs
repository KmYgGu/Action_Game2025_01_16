using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ������ ǥ��.
// �Ǹ� ���� ����
// �Ǹ� ������ ItemShopPopup�� ����.

public class ItemShopSlot : MonoBehaviour
{
    // ������ ������ �θ� �˾� ������ �ʿ�
    private ItemShopPopup popup; // ���� ����(���յ��� ����). ��������(interface) or �븮��(delegate)

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI tradeCountText;

    [SerializeField] private Button leftBTN;
    [SerializeField] private Button rightBTN;

    private int slotIndex;
    public int SlotIndex => slotIndex;//�ܺο����� �бⰡ �����ϵ��� getter�� �����

    private int totalGold;
    public int TotalGold
    {
        get => totalGold;
        set
        {
            totalGold = value;
            // ��������Ʈ

            if(OnTotalChange != null)//�ڽ��� �����ϴ� �����ڵ鿡�� ��ȭ�� �ִٰ� �˸��� ��
                OnTotalChange.Invoke();
        }
    }

    public delegate void TotalGoldChange();
    public event TotalGoldChange OnTotalChange;


    private int tradeGold; // 1�� �ܰ�
    private int tradeMaxCount; // �ŷ� ������ �ִ� ����
    private int tradeCurCount; // �ŷ��Ϸ��� ����� ����
    private int itemID;
    private InventoryItemData data; // � �������� ��� ������ �ִ���

    private void Awake()
    {
        leftBTN.onClick.AddListener(OnClickLeftBTN);
        rightBTN.onClick.AddListener(OnClickRightBTN);
    }


    // itemshopPopup���� ȣ��
    public void CreateSlot(ItemShopPopup owner, int index)
    {
        popup = owner;
        slotIndex = index;
        icon.enabled = false;
        gameObject.SetActive(false);
    }
    // ������ ǥ�⸦ ����

    public void RefreshSlot(InventoryItemData data)
    {
        gameObject.SetActive(true);
        itemID = data.itemID;
        tradeMaxCount = data.amount;
        tradeCurCount = 0;
        TotalGold = 0;// ��������Ʈ�� ���� �κ�ũ�ϱ� ���ؼ� �빮�� ���

        if(DataManager.Inst.GetItemData(itemID, out ItemData_Entity tableInfo))
        { 
            icon.sprite = Resources.Load<Sprite>(tableInfo.iconImg); // << �δ��� ū �۾�, DP����
            icon.enabled = true;
            itemPriceText.text = tableInfo.sellGold.ToString();
            tradeGold = tableInfo.sellGold;
            tradeCountText.text = "0";
            tradeCurCount = 0;
        }
        else
        {
            Debug.Log("�����ۼ󽽷Կ��� RefreshSlot�Լ��� ���̺� ���� ���� ����");
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

    // �ŷ� �� �� �ش� ������ ������ ����.
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

        // uiǥ�� ����
        tradeCountText.text = tradeCurCount.ToString();
        totalGold = tradeGold * tradeCurCount;
    }

    private void OnClickRightBTN()
    {
        if(tradeCurCount < tradeMaxCount)
            tradeCurCount++;

        // uiǥ�� ����
        tradeCountText.text = tradeCurCount.ToString();
        totalGold = tradeGold * tradeCurCount;
    }
}
