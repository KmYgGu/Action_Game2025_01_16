using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst { get; private set; }
    //내부적으로는 쓰기만 할 수 있고, 외부적으론 읽기가 가능하다

    protected virtual void Awake()
    {
        if (Inst == null)
        {
            Inst = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DoAwake();
    }

    protected virtual void DoAwake()// 파생 클래스에서의 초기화가 필요할 때 활용
    {

    }
}

public class SingletonDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst { get; private set; }
    //내부적으로는 쓰기만 할 수 있고, 외부적으론 읽기가 가능하다

    protected virtual void Awake()
    {
        if (Inst == null)
        {
            Inst = this as T;
            //DontDestroyOnLoad(gameObject); 하지 않는다
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DoAwake();
    }

    protected virtual void DoAwake()// 파생 클래스에서의 초기화가 필요할 때 활용
    {

    }
}
