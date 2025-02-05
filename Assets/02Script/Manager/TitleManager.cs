using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private GameObject nickNamePopup;
    [SerializeField] private TextMeshProUGUI errorText;

    private bool havePlayerInfo;

    private void Start()//Awake()
    {
        InitTitleScene();

        DataManager.Inst.InitManager();//게임 전체 사용하는 데이터 매니저 초기화.
    }
    //유저 데이터 있는지 확인
    // y : 터치했을 때 다음 씬으로 진입.
    // n : 터치하면, 계정생성


    private void InitTitleScene()
    {
        if (GameManager.Inst.TryGetPlayerData()) // 유저 데이터 있다면
        {
            welcomeText.text = $"{GameManager.Inst.PlayerNickName}님 환영합니다. \n터치시 시작.";
            havePlayerInfo = true ;
        }
        else
        {
            welcomeText.text = "계속할려면 터치 하세요";
            havePlayerInfo= false ;
        }
    }

    public void EnterBTN()
    {
        if (havePlayerInfo)
        {
            GameManager.Inst.AsyncLoadNextScene(SceneName.BaseScene);
        }
        else// 첫 방문 유저
        {
            LeanTween.scale(nickNamePopup, Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
            welcomeText.enabled = false;
           
        }
        //Debug.Log("화면 클릭");

    }

    public void DeleteBTN()
    {
        GameManager.Inst.DeleteData(); // 데이터 삭제
        InitTitleScene(); // 화면 갱신

        //Debug.Log("삭제 클릭");
    }

    private string newNickName;
    public void InputFieldNickName(string input)
    {
        newNickName = input;
    }

    public void ApplyBTN()
    {
        if(null != newNickName && newNickName.Length >=2)
        {
            LeanTween.scale(nickNamePopup, Vector3.zero, 0.7f).setEase(LeanTweenType.easeOutElastic);
            GameManager.Inst.CreateUserData(newNickName);
            GameManager.Inst.SaveData();
            InitTitleScene();
            welcomeText.enabled = true;
        }
        else// 닉네임의 갯수를 적게 입력했거나, 입력하지 않았을 때
        {
            // 에러 메세지.
            WarningText();

        }

        //Debug.Log("적용 클릭");
    }

    #region _WarningText_
    private void WarningText()
    {
        Color fromColor = Color.red;
        fromColor.a = 0f;
        Color toColor = Color.red;
        toColor.a = 1f;

        LeanTween.value(errorText.gameObject, UpdateValue, fromColor, toColor, 1f)
            .setEase(LeanTweenType.easeInQuad);
        LeanTween.value(errorText.gameObject, UpdateValue, toColor, fromColor, 1f)
            .setDelay(2.0f).setEase(LeanTweenType.easeInQuad);
    }

    private void UpdateValue(Color color)
    {
        errorText.color = color;
    }
    #endregion

}
