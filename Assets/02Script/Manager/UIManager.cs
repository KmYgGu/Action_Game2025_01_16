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
        InitManager();//추후 게임 매니저 혹은 씬매니저에서 관리
    }

    public void InitManager()
    {
        obj = GameObject.Find("AttackBtn");
        if(obj != null )
        {
            if(obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) 람다식 : 익명함수
                btn.onClick.AddListener(()=>HandleButtonClick(ButtonType.BT_AttackBtn));
            }
        }
        obj = GameObject.Find("SkillBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) 람다식 : 익명함수
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_Skiil01Btn));
            }
        }

        obj = GameObject.Find("SkillBtn1");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) 람다식 : 익명함수
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_Skiil02Btn));
            }
        }
        obj = GameObject.Find("SkillBtn2");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) 람다식 : 익명함수
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_Skiil03Btn));
            }
        }
        obj = GameObject.Find("MenuBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) 람다식 : 익명함수
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_MenuBtn));
            }
        }
        obj = GameObject.Find("InventoryBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //()=>HandleButtonClick(ButtonType.BT_MenuBtn) 람다식 : 익명함수
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_InventoryBtn));
            }
        }
        obj = GameObject.Find("SkillBookBtn");
        if (obj != null)
        {
            if (obj.TryGetComponent<Button>(out btn))
            {
                //1회성 메소드는 이름이 필요가 없다
                //=> : 익명함수의 바디를 구현하는 개념
                btn.onClick.AddListener(() => HandleButtonClick(ButtonType.BT_SkiilBookBtn));
            }
        }

        if(InventtoryObj == null)
        {
            InventtoryObj = GameObject.Find("Inventory");
            if(InventtoryObj && !InventtoryObj.TryGetComponent<InventoryUI>(out inventoryUI))
            {
                Debug.Log("UI매니저에서 인벤토리 오브젝트는 존재하지만, 그 안에 inventoryUI 코드가 없습니다");
            }

            InventtoryObj.LeanScale(Vector3.zero, 0.1f);
            isOpenInventory = false;

        }

    }

    public void StartManager()
    {
        
    }

    // 1. 버튼의 핸들러를 한곳에서 관리하는 방식 - 커멘드 패턴 변형
    // 2. 람다식 
    private void HandleButtonClick(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.BT_AttackBtn:
                Debug.Log("공격");
                break;
            case ButtonType.BT_SkiilBookBtn:
                Debug.Log("스킬트리");
                break;
            case ButtonType.BT_Skiil01Btn:
                Debug.Log("스킬1");
                break;
            case ButtonType.BT_Skiil02Btn:
                Debug.Log("스킬2");
                break;
            case ButtonType.BT_Skiil03Btn:
                Debug.Log("스킬3");
                break;
            case ButtonType.BT_MenuBtn:
                Debug.Log("메뉴");
                break;
            case ButtonType.BT_InventoryBtn:
                ShowInventory();
                //Debug.Log("가방");
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
            //inventoryUI.리플레쉬 인벤토리
            inventoryUI.RefreshInventoryUI();
        }
        else
        {
            InventtoryObj.LeanScale(Vector3.zero, 0.7f).setEase(LeanTweenType.easeInElastic);
        }
    }
}
