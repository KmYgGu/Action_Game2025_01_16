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
    private Animator anims;

    private Vector3 camForward;
    private Vector3 camRight;
    private FixedJoystick joystick;

    private GameObject obj;
    #endregion


    #region _public

    public Animator Anims
    {
        get
        {
            if(anims == null)
            {
                anims = transform.GetComponentInChildren<Animator>();
            }

            return anims;
        }
    }
    #endregion

    // 자주 쓰는 애니메이션 문자열을 중복되지 않는 정수의 값으로 변환
    // 전역 선언
    #region _AnimHash_
    private static int animsParam_isWalk = Animator.StringToHash("isWalk");

    #endregion

    private void Awake()
    {
        if(!TryGetComponent<CharacterController>(out controller))
        {
            Debug.Log("AG_Controler.cs - Awake() - 컨트롤러 참조 실패");
        }

        //gameObject.AddComponent<Animator>(); 런타임에 컴포넌트를 추가 혹은 제거

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

        
        if(moveDelta != Vector3.zero)
        {
            transform.forward = moveDelta;
            //자기자신의 앞방향을 moveDelta로 쓰겠다
        }

        //문자열로 찾으면 문자열이 길수록, 할당하는 메모리가 커짐
        Anims?.SetBool(animsParam_isWalk, moveDelta != Vector3.zero);

        controller.Move(moveDelta * (moveSpeed* Time.deltaTime));
    }



}
