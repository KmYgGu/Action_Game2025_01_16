using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���̳��� ���α׷��� (DP �˰���)
// ĳ���ؼ� ����

internal static class YieldInstructionCache//������ ������ ������ �������� ����
{ 
    //readonly public������ �������� ���ϰ�
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    public static readonly Dictionary<float, WaitForSeconds> waitForSeconds =
        new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        //��Ǿ�� Ȯ�� �ߴ��� seconds�� �����ϸ� out�� ���� �����´�
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
        {
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        }

        return wfs;
    }
}
