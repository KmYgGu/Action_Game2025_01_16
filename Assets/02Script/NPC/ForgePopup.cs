using System.Collections;
using System.Collections.Generic;
using System.Linq; // C#����� ���̺귯���� ���� ���� ����

using UnityEngine;
using UnityEngine.UI;
using TMPro;


// ��, Ȱ, ���� ����, �Ķ� ����


// ���ü� ���� ������ �������� ����� ������ ����
// ������ ����Ʈ�� ȭ�鿡 ǥ��
public class ForgePopup : MonoBehaviour, IPopUp
{
    [SerializeField] private Image iconImg; // ���� â�� icon
    [SerializeField] private TextMeshProUGUI enchantInfoText;
    [SerializeField] private TextMeshProUGUI enchantPriceText;
    [SerializeField] private TextMeshProUGUI playerBalanceText;
    [SerializeField] private Button tryBTN;

    [SerializeField] private GameObject forfeSlotPrefab;
    [SerializeField] private RectTransform contentRect;


    private List<ForgeSlot> slotList = new List<ForgeSlot>();
    private InventoryData inventory;
    private ForgeSlot slot;
    private List<InventoryItemData> dataList; // ���ü� �˾�â�� ǥ��Ǿ���ϴ� �������� ��� (�������� �������� �Ҹ�ǰ�� ǥ������ ����)
    private ItemData_Entity tableData;
    private InventoryItemData selectItem;

    private GameObject obj;

    public void PopupClose()
    {
       gameObject.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInElastic); 
    }

    public void PopupOpen()
    {
        RefreshData();
        gameObject.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInElastic);
    }


    private void Start()
    {
        // �ӽ� ȣ��
        Invoke("InitPopup", 0.5f);
    }

    // �˾����� ����ϴ� ������ ����
    private void InitPopup()
    {
        inventory = GameManager.Inst.INVEN;

        for (int i = 0; i < inventory.MaxCount; i++)
        {
            obj = Instantiate(forfeSlotPrefab, contentRect);
            if(obj.TryGetComponent<ForgeSlot>(out slot))
            {
                slot.gameObject.name = $"ForgeSlot_{i}";
                slot.CreateSlot();
                slot.OnselectData += SelectItem;
                slotList.Add(slot);
            }
        }

        tryBTN.onClick.AddListener(OnClick_Enchant);
    }


    // �˾��� ���� �� ���� �ʱ�ȭ
    private void RefreshData()
    {
        iconImg.enabled = false;
        playerBalanceText.text = $"���� ��� : { GameManager.Inst.PlayerGold.ToString()}";
        enchantInfoText.text = "��ȭ 1 -> 2";
        enchantPriceText.text = "��ȭ��� : 0";

        // ������ ������ �ִ� �κ��丮 ������ �����ͼ�
        // �� �� ���������� �����۸� ǥ��

        //dataList = inventory.GetItemList(); // ���� ����
        // ���� ���� ( ��������� ���簡 �ƴ� �Ȱ��� �����͸� 1�� �� ����� ���)

        dataList = inventory.GetItemList().ToList<InventoryItemData>(); // ���� ���� ����Ʈ

        for(int i = dataList.Count - 1; i >= 0; i--)
        {
            if(DataManager.Inst.GetItemData(dataList[i].itemID, out tableData))
            {
                if (!tableData.equip)// ������ �Ұ����� �������� ��쿡��
                {
                    dataList.RemoveAt(i); // ����Ʈ���� Ư���� index�� �����ͳ�带 ����.
                }
            }
        }

        for(int i = 0; i < slotList.Count; i++)
        {
            if(i < dataList.Count)// �������� �ִ� ����
            {
                slotList[i].RefreshSlot(dataList[i]);
            }
            else // �󽽷�
            {
                slotList[i].ClearSlot();
            }
        }
    }

    // ������ â���� Ư�� �������� �������� ���� ó��
    // ���� â���ٰ� ǥ�� ���ִ� �۾�
    public void SelectItem(InventoryItemData selectData)
    {
        
        for(int i =0; i < dataList.Count; i++)
        {
            if (dataList[i].uID == selectData.uID)
            {
                selectItem = selectData;
                if(DataManager.Inst.GetItemData(selectItem.itemID, out tableData))
                {
                    iconImg.enabled = true;
                    iconImg.sprite = Resources.Load<Sprite>(tableData.iconImg);
                }
                else // ���ҽ��� �̹����� ��ã�� ���
                {
                    Debug.Log("ForgePopUp�ڵ忡�� SelectItem �Լ��� ���̺� �ش� ���̵� ����");
                    iconImg.enabled = false;
                }
                enchantInfoText.text = $"��ȭ {selectItem.itemID%1000} -> {selectItem.itemID%1000 +1}";
                enchantPriceText.text = $"��ȭ ��� : {selectItem.itemID % 1000 * 500}";
                playerBalanceText.text = $"���� ��� : {GameManager.Inst.PlayerGold}";

            }
            else // ���õ��� ���� �����۵�
            {
                slotList[i].IsSelect = false;
            }
        }
    }

    // ��ȭ �õ� ��ư�� ������ ��
    public void OnClick_Enchant()
    {
        if (TryEnchant()) // ��ȭ ����
        {
            selectItem.itemID += 1;
            GameManager.Inst.INVEN.UpdateItemInfo(selectItem);// ���� ������ ����
            SelectItem(selectItem); // ��ȭ�� ������ ������ ����
        }
        else // ��ȭ ����
        {
            Debug.Log("���K ��ȭ ����");
        }
    }

    // ��ȭ�� �������� ������ üũ.
    // ��ȭ Ȯ���� Ȯ���ؼ� �������θ� �����ϴ� �Լ�
    private bool TryEnchant()
    {
        bool isScuccess = false;

        if (CanEnchant()) // ��ȭ�� �õ��� �� �ִ� ����
        {
            isScuccess = Random.Range(0, 100000) < 50000; // ���� Ȯ���� ��
            GameManager.Inst.PlayerGold -= (selectItem.itemID % 1000 * 500);
            RefreshData(); // �˾�â ����
        }
        return isScuccess;
    }

    // ��ȭ �������� ������ üũ�ϴ� �Լ�
    private bool CanEnchant()
    {
        if(selectItem.itemID % 1000 >= 5)
        {
            return false; // �ִ�ġ���� ��ȭ�� �Ϸ�� ���
        }

        if(selectItem.itemID % 1000 * 500 > GameManager.Inst.PlayerGold)
        {
            return false; // ����� ��尡 ���� ���
        }

        return true;
    }


}
