using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    [SerializeField] IManager dataManager;
    [SerializeField] IManager gameManager;

    private void Start()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<IManager>();       
        dataManager.InitManager(); //이미 사전에 등록되었습니다 오류는 위의 두줄을 주석처리 2025.02.05

        gameManager = GameObject.Find("GameManager").GetComponent<IManager>();
        gameManager.InitManager(); // ++

        gameManager.StartManager();

        GameManager.Inst.LoadData(); // 세이브 파일 로딩
    }
}
