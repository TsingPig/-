using System.Collections.Generic;
using TsingPigSDK;
using UnityEngine;

public static class MyExtensions
{
    public static T GetRandomItem<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        if (list == null || list.Count == 0)
        {
            Log.Error($"{typeof(T).Name}�б�δ��ʼ�����߳���Ϊ0");
            return default(T); // ���߸���ʵ�����󷵻غ��ʵ�Ĭ��ֵ
        }
        return list[index];
    }
}
