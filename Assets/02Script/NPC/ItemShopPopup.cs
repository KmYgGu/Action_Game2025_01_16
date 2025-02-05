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

                sellSlotList.Add(itemShopSlot);
            }
        }

        for(int i = 0; i < 4; i++)// ��� ���������� 4������
        {
            if(Instantiate(slotPrefab, buyViewContent).TryGetComponent<ItemShopSlot>(out itemShopSlot))
            {
                itemShopSlot.gameObject.name = $"BuySlotM{i}";
                // todo : ������ ������ ������ �޾ƿ��� ��������Ʈ ü�� ����, ���Ͻ�ũ��Ʈ �ۼ� �Ŀ�

                buySlotList.Add(itemShopSlot);
            }
        }

        sellTapBtn.onClick.AddListener(OnClick_Selltap);
        buyTapBtn.onClick.AddListener(OnClick_Buytap);
        applyBtn.onClick.AddListener(OnClick_Apply);
    }

    public void OnClick_Selltap()
    {
        //sellView ����
        sellView.SetActive(true);
        buyView.SetActive(false);
    }
    public void OnClick_Buytap()
    {
        //buyView ����
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
        OnClick_Buytap();// buyTapȭ�� ����
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInOutElastic);
    }
}
