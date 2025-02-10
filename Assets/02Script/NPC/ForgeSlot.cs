using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForgeSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selecticon;
    private bool isSelect;
    public bool IsSelect
    {
        set
        {
            isSelect = value;
            selecticon.enabled = value;
        }
    }
    private InventoryItemData data;
    private Button selectBTN;


    // ������ Ŭ���Ǿ��� ��.
    // �ش� �κ��丮�����۵����� �� �ܺο� ��ǥ
    public delegate void SelectData(InventoryItemData selectData);
    public event SelectData OnselectData;

    public void CreateSlot()
    {
        if(TryGetComponent<Button>(out selectBTN))
        {
            selectBTN.onClick.AddListener(OnClick_SelectBTN);
            gameObject.SetActive(false);
        }
    }
    public void OnClick_SelectBTN()
    {
        if (!isSelect)// ��Ȱ��ȭ �Ǿ��ִ� ������ Ŭ���� ���
        {
            if (OnselectData != null)
                OnselectData(data);
            IsSelect = true;
        }
    }
    public void RefreshSlot(InventoryItemData newData)
    {
        gameObject.SetActive(true);

        data = newData;

        if(DataManager.Inst.GetItemData(data.itemID, out ItemData_Entity tableData))
        {
            icon.enabled = true;
            icon.sprite = Resources.Load<Sprite>(tableData. iconImg );
            isSelect = false;
        }
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }

}
