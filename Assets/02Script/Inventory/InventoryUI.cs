using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ������ �� ���Կ�����Ʈ�� ����.
// �κ��丮�� ���� ��, ������ ������� �ؼ� ������ ������ ����.

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform contectTrans;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventorySlot slot;

    private int currentCount;
    private int maxCount;

    private List<InventoryItemData> dataList; //������ ������ �ִ� ������ ����� ����(���� �Ŵ����� pDATA)

    private void Awake()
    {
        InitSlot();

    }

    // ������ �����ϰ�, �ʱ�ȭ
    private void InitSlot()
    {
        maxCount = 18;
        for(int i = 0; i < maxCount; i++)
        {
            //������ ������Ʈ���� ������ �����´�
            if (Instantiate(slotPrefab, contectTrans).TryGetComponent<InventorySlot>(out slot))
            {
                slot.SLOTINDEX = i;
                slots.Add(slot);
            }
            else
                Debug.Log("�κ��丮UI�ڵ忡�� InitSlot�Լ��� ���� ��������");
        }
    }

    // �� ������ ������ �����ؼ�, ������ ���� ����
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
            else // ��ĭ ����
            {
                slots[i].ClearSlot();
            }
            slots[i].SetSelectSlot(false); // ���õ��� ���� �������� ����
        }
    }
}

