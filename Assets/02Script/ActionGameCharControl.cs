using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ǲ �޾Ƽ� �̵� ����� ����
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
            Debug.Log("AG_Controler.cs - Awake() - ��Ʈ�ѷ� ���� ����");
        }

        obj = GameObject.Find("Joystick");
        if(obj != null )
        {
            obj?.TryGetComponent<FixedJoystick>(out joystick);
        }
    }

    private void Update()
    {
        // Ű���� �Է�ó��
        moveDelta.x = Input.GetAxis("Horizontal");//�Է°��� �޾ƿ���
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
