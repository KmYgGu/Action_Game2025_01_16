using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tipText;

    private void Awake()
    {
        // Awake�� �ɶ� ���� �Ŵ����κ��� ���� ���� ������ Ȯ���ϰ�
        // ���� ���� ���� �����ͷε��� ����

        tipText.text = DataManager.Inst.GetTipMessage(GameManager.Inst.NextSceneName);

        Invoke("NextScenceLoad", 3f);
    }

    private void NextScenceLoad()
    {
        SceneManager.LoadScene(GameManager.Inst.NextSceneName.ToString());
    }
}
