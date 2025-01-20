using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다이나믹 프로그래밍 (DP 알고리즘)
// 캐싱해서 쓴다

internal static class YieldInstructionCache//누구나 접근이 가능한 전역에다 선언
{ 
    //readonly public이지만 수정하지 못하게
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    public static readonly Dictionary<float, WaitForSeconds> waitForSeconds =
        new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        //딕션어리에 확인 했더니 seconds가 존재하면 out를 통해 내놓는다
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
        {
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        }

        return wfs;
    }
}
