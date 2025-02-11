using System.Collections;
using System.Collections.Generic;
using System.Linq; // C#언어의 라이브러리를 통해 깊은 복사

using UnityEngine;
using UnityEngine.UI;
using TMPro;


// 검, 활, 빨간 물약, 파란 물약


// 제련소 장착 가능한 아이템의 목록을 별도로 생성
// 별도의 리스트를 화면에 표기
public class ForgePopup : MonoBehaviour, IPopUp
{
    [SerializeField] private Image iconImg; // 왼쪽 창의 icon
    [SerializeField] private TextMeshProUGUI enchantInfoText;
    [SerializeField] private TextMeshProUGUI enchantPriceText;
    [SerializeField] private TextMeshProUGUI playerBalanceText;
    [SerializeField] private Button tryBTN;

    [SerializeField] private GameObject forfeSlotPrefab;
    [SerializeField] private RectTransform contentRect;


    private List<ForgeSlot> slotList = new List<ForgeSlot>();
    private InventoryData inventory;
    private ForgeSlot slot;
    private List<InventoryItemData> dataList; // 제련소 팝업창에 표기되어야하는 아이템의 목록 (장착템은 보여지고 소모품은 표시하지 않음)
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
        // 임시 호출
        Invoke("InitPopup", 0.5f);
    }

    // 팝업에서 사용하는 슬롯을 생성
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


    // 팝업이 열릴 때 정보 초기화
    private void RefreshData()
    {
        iconImg.enabled = false;
        playerBalanceText.text = $"보유 골드 : { GameManager.Inst.PlayerGold.ToString()}";
        enchantInfoText.text = "강화 1 -> 2";
        enchantPriceText.text = "강화비용 : 0";

        // 유저가 가지고 있는 인벤토리 정보를 가져와서
        // 그 중 장착가능한 아이템만 표기

        //dataList = inventory.GetItemList(); // 얕은 복사
        // 깊은 복사 ( 참조방식의 복사가 아닌 똑같은 데이터를 1개 더 만드는 방식)

        dataList = inventory.GetItemList().ToList<InventoryItemData>(); // 깊은 복사 리스트

        for(int i = dataList.Count - 1; i >= 0; i--)
        {
            if(DataManager.Inst.GetItemData(dataList[i].itemID, out tableData))
            {
                if (!tableData.equip)// 장착이 불가능한 아이템인 경우에는
                {
                    dataList.RemoveAt(i); // 리스트에서 특정한 index의 데이터노드를 제거.
                }
            }
        }

        for(int i = 0; i < slotList.Count; i++)
        {
            if(i < dataList.Count)// 아이템이 있는 슬롯
            {
                slotList[i].RefreshSlot(dataList[i]);
            }
            else // 빈슬롯
            {
                slotList[i].ClearSlot();
            }
        }
    }

    // 오른쪽 창에서 특정 아이템을 선택했을 때의 처리
    // 왼쪽 창에다가 표기 해주는 작업
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
                else // 리소스에 이미지를 못찾은 경우
                {
                    Debug.Log("ForgePopUp코드에서 SelectItem 함수의 테이블에 해당 아이디가 없음");
                    iconImg.enabled = false;
                }
                enchantInfoText.text = $"강화 {selectItem.itemID%1000} -> {selectItem.itemID%1000 +1}";
                enchantPriceText.text = $"강화 비용 : {selectItem.itemID % 1000 * 500}";
                playerBalanceText.text = $"보유 골드 : {GameManager.Inst.PlayerGold}";

            }
            else // 선택되지 못한 아이템들
            {
                slotList[i].IsSelect = false;
            }
        }
    }

    // 강화 시도 버튼이 눌렸을 때
    public void OnClick_Enchant()
    {
        if (TryEnchant()) // 강화 성공
        {
            selectItem.itemID += 1;
            GameManager.Inst.INVEN.UpdateItemInfo(selectItem);// 원본 데이터 변경
            SelectItem(selectItem); // 변화된 아이템 정보로 갱신
        }
        else // 강화 실패
        {
            Debug.Log("하핳 강화 실패");
        }
    }

    // 강화가 가능한지 조건을 체크.
    // 강화 확률을 확인해서 성공여부를 생성하는 함수
    private bool TryEnchant()
    {
        bool isScuccess = false;

        if (CanEnchant()) // 강화를 시도할 수 있는 상태
        {
            isScuccess = Random.Range(0, 100000) < 50000; // 성공 확률은 반
            GameManager.Inst.PlayerGold -= (selectItem.itemID % 1000 * 500);
            RefreshData(); // 팝업창 갱신
        }
        return isScuccess;
    }

    // 강화 가능한지 조건을 체크하는 함수
    private bool CanEnchant()
    {
        if(selectItem.itemID % 1000 >= 5)
        {
            return false; // 최대치까지 강화가 완료된 경우
        }

        if(selectItem.itemID % 1000 * 500 > GameManager.Inst.PlayerGold)
        {
            return false; // 충분한 골드가 없는 경우
        }

        return true;
    }


}
