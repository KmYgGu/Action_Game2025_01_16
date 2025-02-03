using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// �κ��丮�� ������������ ǥ��
// � �������� � ������ �ִ���..
public class InventorySlot : MonoBehaviour
{
    private bool isEmpty;

    public bool EMPTY
    {
        get => isEmpty;
    }

    private int slotindex; // �� ��° ���������� �����ִ� index��
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

    // ������ ������ �־ icon�� amount�� ����
    // ������ ������ �ʿ�
    public void DrawItemSlot(InventoryItemData itemData)
    {
        if(DataManager.Inst.GetItemData(itemData.itemID, out ItemData_Entity itemInfo))
        {
            // ���� �ε��� ���ؼ� icon�� ������ ����
            icon.sprite = Resources.Load<Sprite>(itemInfo.iconImg); // ���� ��θ� ������� ���� �ε�
            icon.enabled = true;
            ChangeAmount(itemData.amount);  // �������� ����
            isEmpty = false;
        }
        else
        {
            Debug.Log("InventorySlot.cs �����۽��Ի��� �Լ��� ���̺� ������ ���� ���̵�");
        }
    }

    // �����ϰ� �ִ� �������� ���� / �ǸŰ� �Ǿ��� �� ������ ���� ����
    public void ClearSlot()
    {
        isSelect = false;
        isEmpty = true;
        focus.SetActive(false);
        amount.enabled = false;
        icon.enabled = false;
    }
    // ���� ������ ����
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

    // ������ ������ ���θ� ����
    public void SetSelectSlot(bool isSelected)
    {
        focus.SetActive(isSelected);
        this.isSelect = isSelected;
    }

    public void OnClick_Select()
    {
        if (!isEmpty) // �󽽷��� �ƴ� ��
        {
            isSelect = !isSelect;
            SetSelectSlot(isSelect);
        }
    }

}
