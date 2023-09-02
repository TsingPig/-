using System.Collections.Generic;
using System.Linq;
using TsingPigSDK;
using UnityEngine.AI;

public class InstrumentManager : Singleton<InstrumentManager>
{
    private const float w1 = 0.2f;

    private const float w2 = 0.8f;

    private List<InstrumentInfo> _instrumentInfos;

    private Dictionary<string, List<Instrument>> _dicInstruments = new Dictionary<string, List<Instrument>>();

    /// <summary>
    /// �豸��ʼ����˫��ע�ᣨInfo�󶨵��豸���豸�󶨵���������
    /// </summary>
    /// <param name="instrument">�豸�ű�</param>
    /// <returns></returns>
    public InstrumentInfo GetInfo(Instrument instrument)
    {
        string instrumentID = instrument.name;
        instrumentID = instrumentID.Substring(0, instrumentID.IndexOf(" ") == -1 ? instrumentID.Length : instrumentID.IndexOf(" "));
        InstrumentInfo info = _instrumentInfos.Find(info => info.instrumentID == instrumentID);

        if (_dicInstruments.ContainsKey(info.instrumentID))
        {
            _dicInstruments[info.instrumentID].Add(instrument);
        }
        else
        {
            List<Instrument> lstInstrument = new List<Instrument>() { instrument };
            _dicInstruments.Add(info.instrumentID, lstInstrument);
        }
        Log.Info($"ID : {instrumentID}ע��{info.instrumentName}");
        return info;
    }

    /// <summary>
    /// �Ƽ��㷨������������Ϣ���ƶϴ�����ʵ��豸�Լ�������Ŀ��
    /// </summary>
    /// <param name="inspectionInfos">������Ϣ</param>
    /// <param name="agent">��ǰ���˵�������</param>
    /// <returns></returns>
    public Instrument Recommend(List<InspectionInfo> inspectionInfos, NavMeshAgent agent)
    {
        List<InstrumentInfo> instrumentInfos = new List<InstrumentInfo>();
        foreach (var inspcInfo in inspectionInfos)
        {
            instrumentInfos.Add(GetInfo(inspcInfo.instrumentID));
        }
        List<Instrument> instruments = new List<Instrument>();

        foreach (var instrInfo in instrumentInfos)
        {
            List<Instrument> instrs = _dicInstruments[instrInfo.instrumentID];
            instruments.AddRange(instrs);
        }
        instruments.OrderBy(instr => GetAttraction(instr, agent));
        Log.Info($"�Ƽ� {instruments[0].InstrumentInfo.instrumentName}");
        return instruments[0];
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
    private void Init()
    {
        Instrument_SO instrumentData = Res.Load<Instrument_SO>(Str_Def.INSTRUMENT_DATA_PATH);
        _instrumentInfos = instrumentData.instrumentInfos;

    }

    /// <summary>
    /// �����豸ID����ù�������Info
    /// </summary>
    /// <param name="instrumentID"></param>
    /// <returns></returns>
    private InstrumentInfo GetInfo(string instrumentID)
    {
        return _instrumentInfos.Find(info => info.instrumentID == instrumentID);
    }

    /// <summary>
    /// ����ĳ̨�豸��ÿ�����˵������̶�
    /// </summary>
    /// <param name="instrument">�豸�ű�</param>
    /// <param name="agent">����������</param>
    /// <returns></returns>
    private float GetAttraction(Instrument instrument, NavMeshAgent agent)
    {
        float waitingTime = instrument.WaitingTime;
        float pathLength = MyExtensions.CalculatePathLength(agent.transform, instrument.transform);
        float pathTime = pathLength / agent.velocity.magnitude;
        return w1 * waitingTime + w2 * pathTime;
    }

}
