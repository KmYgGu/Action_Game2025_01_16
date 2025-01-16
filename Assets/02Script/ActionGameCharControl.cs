using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인풋 받아서 이동 기능을 구현
public class ActionGameCharControl : MonoBehaviour
{
    #region _SerializeField_
    [SerializeField] private float moveSpeed = 6f;
    #endregion
    #region _Private_
    private Vector3 moveDelta;
    private CharacterController controller;
    private Vector3 camForward;
    private Vector3 camRight;
    private FixedJoystick joystick;

    private GameObject obj;
    #endregion
    private void Awake()
    {
        if(!TryGetComponent<CharacterController>(out controller))
        {
            Debug.Log("AG_Controler.cs - Awake() - 컨트롤러 참조 실패");
        }

        obj = GameObject.Find("Joystick");
        if(obj != null )
        {
            obj?.TryGetComponent<FixedJoystick>(out joystick);
        }
    }

    private void Update()
    {
        // 키보드 입력처림
        moveDelta.x = Input.GetAxis("Horizontal");//입력값을 받아오는
        moveDelta.y = 0.0f;
        moveDelta.z = Input.GetAxis("Vertical");

        //moveDelta.x += joystick.Horizontal;
        //moveDelta.z += joystick.Vertical;
        moveDelta.Normalize();


        camForward = Camera.main.transform.forward;
        camForward.y = 0.0f;
        camForward.Normalize();

        camRight = Camera.main.transform.right;
        camRight.y = 0.0f;
        camRight.Normalize();

        moveDelta = camForward * moveDelta.z + camRight * moveDelta.x;
        moveDelta.Normalize();

        controller.Move(moveDelta * (moveSpeed* Time.deltaTime));
    }



}
