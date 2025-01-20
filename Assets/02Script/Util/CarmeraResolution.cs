using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 모바일 기기에서 제작할 때 보았던 가로세로 비율을 표현하기 위해서
// 레터박스 방식.
public class CarmeraResolution : MonoBehaviour
{
    private void Awake()
    {
        if(TryGetComponent<Camera>(out Camera cam))
        {
            Rect rt = cam.rect;
            float scale_Height = ((float)Screen.width / Screen.height) / ((float)16 / 9);
            //실제 플레이하는 모니터에 만들려는 해상도의 비율을 가져올 수 있다

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
