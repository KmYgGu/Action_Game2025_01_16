using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 인벤토리의 아이템정보를 표기
// 어떤 아이템을 몇개 가지고 있는지..
public class InventorySlot : MonoBehaviour
{
    private bool isEmpty;

    public bool EMPTY
    {
        get => isEmpty;
    }

    private int slotindex; // 몇 번째 아이템인지 정해주는 index값
    public int SLOTINDEX
    {
        get =>
            slotindex;
        set =>
            slotindex = value;
    }
    private Image icon;
    private GameObject focus;
    private TextMeshProUGUI amount;
    public Button button;
    private bool isSelect;

    private void Awake()
    {
        transform.GetChild(0).TryGetComponent<Image>(out icon);
        focus = transform.GetChild(1).gameObject;
        transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out amount);
        if (TryGetComponent<Button>(out button))
            button.onClick.AddListener(OnClick_Select);

        ClearSlot();
    }

    // 아이템 정보가 있어서 icon과 amount를 갱신
    // 아이템 정보가 필요
    public void DrawItemSlot(InventoryItemData itemData)
    {
        if(DataManager.Inst.GetItemData(itemData.itemID, out ItemData_Entity itemInfo))
        {
            // 동적 로딩을 통해서 icon의 변경을 수행
            icon.sprite = Resources.Load<Sprite>(itemInfo.iconImg); // 파일 경로를 기반으로 동적 로딩
            icon.enabled = true;
            ChangeAmount(itemData.amount);  // 보류갯수 갱신
            isEmpty = false;
        }
        else
        {
            Debug.Log("InventorySlot.cs 아이템슬롯생성 함수가 테이블 정보가 없는 아이디");
        }
    }

    // 보유하고 있던 아이템이 삭제 / 판매가 되었을 때 슬롯을 비우는 로직
    public void ClearSlot()
    {
        isSelect = false;
        isEmpty = true;
        focus.SetActive(false);
        amount.enabled = false;
        icon.enabled = false;
    }
    // 보유 갯수를 갱신
    public void ChangeAmount(int newAnount)
    {
        if (newAnount < 2)
            amount.enabled = false;
        else
        {
            amount.enabled = true;
            amount.text = newAnount.ToString();
        }
    }

    // 슬롯이 선택의 여부를 변경
    public void SetSelectSlot(bool isSelected)
    {
        focus.SetActive(isSelected);
        this.isSelect = isSelected;
    }

    public void OnClick_Select()
    {
        if (!isEmpty) // 빈슬롯이 아닐 때
        {
            isSelect = !isSelect;
            SetSelectSlot(isSelect);
        }
    }

}
