using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ButtonType
{
    BT_AttackBtn,
    BT_Skiil01Btn,
    BT_Skiil02Btn,
    BT_Skiil03Btn,
    BT_MenuBtn,
    BT_InventoryBtn,
    BT_SkiilBookBtn,

}

public class UIManager : MonoBehaviour, IManager
{
    private GameObject obj;
    private Button btn;

    private GameObject InventtoryObj;
    private InventoryUI inventoryUI;
    private bool isOpenInventory;


    private void Awake()
    {
        InitManager();//���� ���� �Ŵ��� Ȥ�� ���Ŵ������� ����
    }

    public void InitManager()
    {
        obj = GameObject.Find("AttackBtn");
        if(obj != null )
        {
            if(obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) ���ٽ� : �͸��Լ�
                btn.onClick.AddListener(()=>HandleButtonClick(ButtonType.BT_AttackBtn));
            }
        }
        obj = GameObject.Find("SkillBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) ���ٽ� : �͸��Լ�
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_Skiil01Btn));
            }
        }

        obj = GameObject.Find("SkillBtn1");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) ���ٽ� : �͸��Լ�
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_Skiil02Btn));
            }
        }
        obj = GameObject.Find("SkillBtn2");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) ���ٽ� : �͸��Լ�
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_Skiil03Btn));
            }
        }
        obj = GameObject.Find("MenuBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) ���ٽ� : �͸��Լ�
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_MenuBtn));
            }
        }
        obj = GameObject.Find("InventoryBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) ���ٽ� : �͸��Լ�
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_InventoryBtn));
            }
        }
        obj = GameObject.Find("SkillBookBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //1ȸ�� �޼ҵ�� �̸��� �ʿ䰡 ����
                //=> : �͸��Լ��� �ٵ� �����ϴ� ����
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_SkiilBookBtn));
            }
        }

        if(InventtoryObj == null)
        {
            InventtoryObj = GameObject.Find("Inventory");
            if(InventtoryObj && !InventtoryObj.TryGetComponent<InventoryUI>(out inventoryUI))
            {
                Debug.Log("UI�Ŵ������� �κ��丮 ������Ʈ�� ����������, �� �ȿ� inventoryUI �ڵ尡 �����ϴ�");
            }

            InventtoryObj.LeanScale(Vector3.zero, 0.1f);
            isOpenInventory = false;

        }

    }

    public void StartManager()
    {
        
    }

    // 1. ��ư�� �ڵ鷯�� �Ѱ����� �����ϴ� ��� - Ŀ��� ���� ����
    // 2. ���ٽ� 
    private void HandleButtonClick(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.BT_AttackBtn:
                Debug.Log("����");
                break;
            case ButtonType.BT_SkiilBookBtn:
                Debug.Log("��ųƮ��");
                break;
            case ButtonType.BT_Skiil01Btn:
                Debug.Log("��ų1");
                break;
            case ButtonType.BT_Skiil02Btn:
                Debug.Log("��ų2");
                break;
            case ButtonType.BT_Skiil03Btn:
                Debug.Log("��ų3");
                break;
            case ButtonType.BT_MenuBtn:
                Debug.Log("�޴�");
                break;
            case ButtonType.BT_InventoryBtn:
                ShowInventory();
                //Debug.Log("����");
                break;
            default:
                break;
        }
    }

    public void ShowInventory()
    {
        isOpenInventory = !isOpenInventory;

        if (isOpenInventory)
        {
            InventtoryObj.LeanScale(Vector3.one, 0.7f).setEase(LeanTweenType.easeInElastic);
            //inventoryUI.���÷��� �κ��丮
            inventoryUI.RefreshInventoryUI();
        }
        else
        {
            InventtoryObj.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInElastic);
        }
    }
}
