using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst { get; private set; }
    //���������δ� ���⸸ �� �� �ְ�, �ܺ������� �бⰡ �����ϴ�

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

    protected virtual void DoAwake()// �Ļ� Ŭ���������� �ʱ�ȭ�� �ʿ��� �� Ȱ��
    {

    }
}

public class SingletonDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst { get; private set; }
    //���������δ� ���⸸ �� �� �ְ�, �ܺ������� �бⰡ �����ϴ�

    protected virtual void Awake()
    {
        if (Inst == null)
        {
            Inst = this as T;
            //DontDestroyOnLoad(gameObject); ���� �ʴ´�
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DoAwake();
    }

    protected virtual void DoAwake()// �Ļ� Ŭ���������� �ʱ�ȭ�� �ʿ��� �� Ȱ��
    {

    }
}
