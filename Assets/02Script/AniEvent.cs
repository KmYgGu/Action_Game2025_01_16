using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniEvent : MonoBehaviour
{
    public void AnimEvent_SpownProjTile()
    {
        //Debug.Log("애니메이션 재생에 따른 이벤트 발생");

        // 결합도를 낮춰서 데커플링을 구현한다
        transform.parent.SendMessage("SpawnProjectile");

        // 왠만하면 델리게이트로 해결
    }
}
