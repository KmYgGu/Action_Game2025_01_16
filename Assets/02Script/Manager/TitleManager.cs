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

        DataManager.Inst.InitManager();//���� ��ü ����ϴ� ������ �Ŵ��� �ʱ�ȭ.
    }
    //���� ������ �ִ��� Ȯ��
    // y : ��ġ���� �� ���� ������ ����.
    // n : ��ġ�ϸ�, ��������


    private void InitTitleScene()
    {
        if (GameManager.Inst.TryGetPlayerData()) // ���� ������ �ִٸ�
        {
            welcomeText.text = $"{GameManager.Inst.PlayerNickName}�� ȯ���մϴ�. \n��ġ�� ����.";
            havePlayerInfo = true ;
        }
        else
        {
            welcomeText.text = "����ҷ��� ��ġ �ϼ���";
            havePlayerInfo= false ;
        }
    }

    public void EnterBTN()
    {
        if (havePlayerInfo)
        {
            GameManager.Inst.AsyncLoadNextScene(SceneName.BaseScene);
        }
        else// ù �湮 ����
        {
            LeanTween.scale(nickNamePopup, Vector3.one, 0.7f).setEase(LeanTweenType.easeOutElastic);
            welcomeText.enabled = false;
           
        }
        //Debug.Log("ȭ�� Ŭ��");

    }

    public void DeleteBTN()
    {
        GameManager.Inst.DeleteData(); // ������ ����
        InitTitleScene(); // ȭ�� ����

        //Debug.Log("���� Ŭ��");
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
        else// �г����� ������ ���� �Է��߰ų�, �Է����� �ʾ��� ��
        {
            // ���� �޼���.
            WarningText();

        }

        //Debug.Log("���� Ŭ��");
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
