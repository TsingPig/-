using System.Collections.Generic;
using System.Linq;
using TsingPigSDK;

public class InspectionManager : Singleton<InspectionManager>
{
    private List<InspectionInfo> _inspectionInfos;

    private bool[,] _matrix;
    private int Len => _inspectionInfos.Count;

    /// <summary>
    /// ���ݼ����ĿID��ö�Ӧ�б��е�������
    /// </summary>
    /// <param name="inspectionID">�����ĿID</param>
    /// <returns></returns>
    private int GetIndex(string inspectionID)
    {
        int idx = _inspectionInfos.FindIndex(info => info.inspectionID == inspectionID);
        if (idx == -1)
        {
            Log.Error($"{inspectionID} ������");
            return 0;
        }
        return idx;
    }

    /// <summary>
    /// �������Ϊ0�����нڵ��������
    /// </summary>
    private List<int> GetIndexs => Enumerable.Range(0, Len).Where(j => Enumerable.Range(0, Len).All(i => !_matrix[i, j])).ToList();

    private void LogMatrix()
    {
        //for (int i = 0; i < Len; i++)
        //{
        //    for (int j = 0; j < Len; j++)
        //    {
        //        if (_matrix[i, j])
        //        {
        //         Log.Info($"{_inspectionInfos[i].inspectionName}({_inspectionInfos[i].instrumentID})-->{_inspectionInfos[j].inspectionName}({_inspectionInfos[j].instrumentID})");
        //        }
        //    }
        //}
        foreach (var i in GetIndexs)
        {
            Log.Info($"{_inspectionInfos[i].inspectionName}({_inspectionInfos[i].instrumentID})");
        }
    }

    private async void Init()
    {
        Inspection_SO inspectionData = await Res.LoadAsync<Inspection_SO>(Str_Def.INSPECTION_DATA_PATH);
        _inspectionInfos = inspectionData.inspectionInfos;
        _matrix = new bool[Len, Len];
        for (int i = 0; i < Len; i++)
        {
            for (int j = 0; j < Len; j++)
            {
                _matrix[i, j] = false;
            }
        }
        for (int v = 0; v < Len; v++)
        {
            var preInspectionIDs = _inspectionInfos[v].preInspectionIDs;
            foreach (var preInspectionID in preInspectionIDs)
            {
                int u = GetIndex(preInspectionID);
                _matrix[u, v] = true;
            }
        }
        LogMatrix();
    }

    private new void Awake()
    {
        base.Awake();
        Init();

    }
}

