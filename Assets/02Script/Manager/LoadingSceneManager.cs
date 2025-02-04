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
        // Awake가 될때 게임 매니저로부터 다음 씬이 누군지 확인하고
        // 다음 씬에 따라 데이터로딩을 수행

        tipText.text = DataManager.Inst.GetTipMessage(GameManager.Inst.NextSceneName);

        Invoke("NextScenceLoad", 3f);
    }

    private void NextScenceLoad()
    {
        SceneManager.LoadScene(GameManager.Inst.NextSceneName.ToString());
    }
}
