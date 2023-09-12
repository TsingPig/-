using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TsingPigSDK
{

    public static class MyExtensions
    {
        /// <summary>
        /// ��ȡ�б������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ���㵼��·������
        /// </summary>
        /// <param name="path">����·��</param>
        /// <returns></returns>
        public static float CalculatePathLength(Transform source, Transform target)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(source.position, target.position, NavMesh.AllAreas, path))
            {
                float pathLength = 0;

                if (path.corners.Length < 2)
                {
                    return pathLength;
                }

                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Vector3 unityVector3A = new Vector3(path.corners[i].x, path.corners[i].y, path.corners[i].z);
                    Vector3 unityVector3B = new Vector3(path.corners[i + 1].x, path.corners[i + 1].y, path.corners[i + 1].z);

                    pathLength += Vector3.Distance(unityVector3A, unityVector3B);
                }

                return pathLength;
            }
            else
            {
                return float.MaxValue;
            }


        }
    }
}
