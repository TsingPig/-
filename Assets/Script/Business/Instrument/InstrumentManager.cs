using System.Collections.Generic;
using System.Linq;
using TsingPigSDK;
public class InstrumentManager : Singleton<InstrumentManager>
{
    private Instrument_SO _instrumentData;

    private List<InstrumentInfo> _instrumentInfos;

    private Dictionary<InstrumentInfo, List<Instrument>> _dicInstruments = new Dictionary<InstrumentInfo, List<Instrument>>();
    public List<InstrumentInfo> InstrumentInfos { get => _instrumentInfos; set => _instrumentInfos = value; }

    /// <summary>
    /// �����豸��˫��ע�ᣨInfo�󶨵��豸���豸�󶨵���������
    /// </summary>
    /// <param name="instrument">�豸�ű�</param>
    /// <returns></returns>
    public InstrumentInfo GetInfo(Instrument instrument)
    {
        string instrumentID = instrument.name;
        instrumentID = instrumentID.Substring(0, instrumentID.IndexOf(" ") == -1 ? instrumentID.Length : instrumentID.IndexOf(" "));
        InstrumentInfo info = _instrumentInfos.Find(info => info.instrumentID == instrumentID);

        if (_dicInstruments.ContainsKey(info))
        {
            _dicInstruments[info].Add(instrument);
        }
        else
        {
            List<Instrument> lstInstrument = new List<Instrument>() { instrument };
            _dicInstruments.Add(info, lstInstrument);
        }
        return info;
    }
    public Instrument Recommend(InspectionInfo inspectionInfo)
    {
        InstrumentInfo info = GetInfo(inspectionInfo.instrumentID);
        List<Instrument> lstInstrument = _dicInstruments[info];
        lstInstrument.OrderBy(instr => instr.WaitingTime);
        return lstInstrument.First();
    }
    private new void Awake()
    {
        base.Awake();
        Init();
    }
    private void Init()
    {
        _instrumentData = Res.Load<Instrument_SO>(Str_Def.INSTRUMENT_DATA_PATH);
        _instrumentInfos = _instrumentData.instrumentInfos;

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
}
