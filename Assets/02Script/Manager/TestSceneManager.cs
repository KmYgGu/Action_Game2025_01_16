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
        dataManager.InitManager();

        gameManager = GameObject.Find("GameManager").GetComponent<IManager>();
        gameManager.StartManager();
    }
}
