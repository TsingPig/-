using System.Collections.Generic;
using System.Linq;
using TsingPigSDK;
using UnityEngine.AI;

public class Inspection
{
    private List<InspectionInfo> _inspectionInfos;

    private bool[] _visited;

    private bool[,] _matrix;
    private int Len => _inspectionInfos.Count;

    private InspectionInfo _curInspectionInfo;
    public InspectionInfo CurInspectionInfo { get => _curInspectionInfo; set => _curInspectionInfo = value; }

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
    /// ����Ҫǰ�����豸����ƥ������Ŀ��
    /// </summary>
    /// <param name="indexs">��ǰ���Լ�����ĿID</param>
    /// <param name="instrument">��ǰ���������豸</param>
    /// <returns>�豸��ƥ�䵽��Ӧ����Ŀ</returns>
    private int GetIndex(List<int> indexs, Instrument instrument)
    {
        InstrumentInfo info = instrument.InstrumentInfo;
        //Log.Info($"{info.InspectionIDs.Count}");
        foreach (var idx in indexs)
        {
            foreach (var idItem in instrument.InstrumentInfo.InspectionIDs)
            {
                if (_inspectionInfos[idx].inspectionID == idItem.inspectionID)
                {
                    Log.Info($"��ǰҪǰ��{instrument.InstrumentInfo.instrumentName} ��� {_inspectionInfos[idx].inspectionName}");
                    return idx;
                }
            }
        }
        Log.Error($" {instrument.InstrumentInfo.instrumentName} ƥ��ʧ��");
        return -1;
    }

    /// <summary>
    /// �������Ϊ0�����нڵ��������
    /// </summary>
    private List<int> GetIndexs => Enumerable.Range(0, Len).
        Where(j => Enumerable.Range(0, Len).
        All(i => !_matrix[i, j] && !_visited[j])).
        ToList();

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
    private void Init()
    {
        _inspectionInfos = InspectionManager.Instance.InspectionInfos;
        _visited = new bool[Len];
        _matrix = new bool[Len, Len];
        for (int i = 0; i < Len; i++)
        {
            _visited[i] = false;
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
        //LogMatrix();
    }
    public Inspection()
    {
        Init();
    }

    /// <summary>
    ///  ����������ͼ���Ѱ�����ż���豸��ƥ����Ŀ��
    /// </summary>
    /// <param name="agent">����������</param>
    /// <returns>��һ��ǰ�����豸</returns>
    public Instrument GetNext(NavMeshAgent agent)
    {
        List<int> indexs = GetIndexs;
        if (indexs.Count == 0)
        {
            Log.Info("���м�������");
            return null;
        }
        int curInspectionIdx = 0;
        List<InspectionInfo> infos = new List<InspectionInfo>();
        foreach (var idx in indexs)
        {
            infos.Add(_inspectionInfos[idx]);
        }
        Instrument nextInstrument = InstrumentManager.Instance.Recommend(infos, agent);

        curInspectionIdx = GetIndex(indexs, nextInstrument);
        _visited[curInspectionIdx] = true;
        for (int j = 0; j < Len; j++)
            _matrix[curInspectionIdx, j] = false;
        _curInspectionInfo = _inspectionInfos[curInspectionIdx];
        Log.Info($"��ǰѡ��{_curInspectionInfo.inspectionName}");
        //LogMatrix();
        return nextInstrument;
    }
}

