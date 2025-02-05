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
        dataManager.InitManager(); //�̹� ������ ��ϵǾ����ϴ� ������ ���� ������ �ּ�ó�� 2025.02.05

        gameManager = GameObject.Find("GameManager").GetComponent<IManager>();
        gameManager.InitManager(); // ++

        gameManager.StartManager();

        GameManager.Inst.LoadData(); // ���̺� ���� �ε�
    }
}
