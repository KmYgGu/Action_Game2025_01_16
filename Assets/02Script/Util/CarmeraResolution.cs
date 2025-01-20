using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ����� ��⿡�� ������ �� ���Ҵ� ���μ��� ������ ǥ���ϱ� ���ؼ�
// ���͹ڽ� ���.
public class CarmeraResolution : MonoBehaviour
{
    private void Awake()
    {
        if(TryGetComponent<Camera>(out Camera cam))
        {
            Rect rt = cam.rect;
            float scale_Height = ((float)Screen.width / Screen.height) / ((float)16 / 9);
            //���� �÷����ϴ� ����Ϳ� ������� �ػ��� ������ ������ �� �ִ�

            float scale_width = 1f / scale_Height;

            if(scale_Height < 1f)
            {
                rt.height = scale_Height;
                rt.y = (1f - scale_Height) / 2f;
            }
            else
            {
                rt.width = scale_width;
                rt.x = (1f - scale_width) / 2f;
            }
            cam.rect = rt;
        }
    }
}
