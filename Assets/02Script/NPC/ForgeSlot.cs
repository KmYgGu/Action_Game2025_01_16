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


    // 슬롯이 클릭되었을 때.
    // 해당 인벤토리아이템데이터 를 외부에 공표
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
        if (!isSelect)// 비활성화 되어있는 슬롯을 클릭할 경우
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
